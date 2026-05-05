// 📁 Application/Features/Orders/CreateOrder/CreateOrderHandler.cs
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;
using StoreWebapi.Domain.Interfaces;

public class CreateOrderHandler(
    IRepository repo, 
    IHttpContextAccessor httpContextAccessor,
    IPaymentService paymentService,
    IConfiguration config) 
    : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
{
    public async Task<Result<CreateOrderResponse>> Handle(
        CreateOrderCommand request, 
        CancellationToken cancellationToken)
    {
        // 🔐 1. Authenticate buyer
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) 
            return Result.Failure<CreateOrderResponse>("User not authenticated");
        var buyerId = Guid.Parse(userIdClaim.Value);

        // 📚 2. Validate books exist
        var books = await repo.FindAll<Book>(b => request.BookIds.Contains(b.id), cancellationToken);
        if (books.Count != request.BookIds.Count)
            return Result.Failure<CreateOrderResponse>("One or more books not found");

      

        decimal discount = 1.0m;
        coupons? appliedCoupon = null;

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            // 1. جلب الكوبون بالكود فقط (بدون IsExpired في الاستعلام)
            appliedCoupon = await repo.FindFirstOrDefault<coupons>(
                c => c.code == request.CouponCode, 
                cancellationToken);
    
            // 2. التحقق من الوجود
            if (appliedCoupon == null)
                return Result.Failure<CreateOrderResponse>("الكوبون غير صالح أو غير موجود");

            // 3. التحقق من الصلاحية (هنا IsExpired ستعمل بدون مشاكل)
            if (appliedCoupon.IsExpired)
                return Result.Failure<CreateOrderResponse>("عذراً، هذا الكوبون منتهي الصلاحية");
    
            // 4. التحقق مما إذا كان المستخدم قد استخدمه سابقاً
            var used = await repo.AnyAsync<couponUser>(
                cu => cu.couponId == appliedCoupon.couponId && cu.userId == buyerId, 
                cancellationToken);
    
            if (used)
                return Result.Failure<CreateOrderResponse>("لقد قمت باستخدام هذا الكوبون مسبقاً");
    
            // 5. تطبيق الخصم
            discount = 1m - (appliedCoupon.Discount_percentage / 100m);
        }

        // 🧮 4. Calculate totals & create order
        var orderId = Guid.NewGuid();
        var transactions = new List<transaction>();
        decimal totalAmount = 0;

        foreach (var book in books)
        {
            if (book.UserId == null)
                return Result.Failure<CreateOrderResponse>($"Book '{book.name}' has no vendor");
            
            var discountedPrice = book.price * discount;
            totalAmount += discountedPrice;
            
            transactions.Add(new transaction
            {
                id = Guid.NewGuid(),
                OrderId = orderId,
                userId = buyerId,
                vendorId = book.UserId.Value,
                bookId = book.id,
                amount = discountedPrice,
                currency = "usd",
                Status = PaymentStatus.Pending,
                TransactionDate = DateTime.UtcNow,
                destination = "Digital"
            });
        }

        // 📦 5. Create Order entity (NOT saved yet)
        var order = new Order
        {
            Id = orderId,
            BuyerId = buyerId,
            TotalAmount = totalAmount,
            Status = PaymentStatus.Pending.ToString(), 
            OrderDate = DateTime.UtcNow,
            Destination = "Digital",
            Transactions = transactions,
            StripeSessionId = null 
        };

        // 🆓 6. Handle $0 orders (100% discount)
        if (totalAmount <= 0)
        {
            order.Status = PaymentStatus.Paid.ToString();
            foreach (var t in transactions) t.Status = PaymentStatus.Paid;
            
            repo.Add(order);
            await repo.SaveChangesAsync(cancellationToken);
            
            
            foreach (var t in transactions)
            {
                repo.Add(new UserBook 
                { 
                    UserId = buyerId, 
                    BookId = t.bookId, 
                    PurchaseDate = DateTime.UtcNow 
                });
            }
            await repo.SaveChangesAsync(cancellationToken);
            
            return Result.Success(new CreateOrderResponse(orderId, string.Empty, IsFree: true));
        }

        // 💳 7. Create Stripe Checkout Session
        var baseUrl = config["App:BaseUrl"] ?? "http://localhost:5173";
        var stripeResult = await paymentService.CreateCheckoutSessionAsync(
            order, 
            transactions,
            successUrl: $"{baseUrl}/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
            cancelUrl: $"{baseUrl}/payment/cancel"
        );
        
        if (stripeResult.IsFailure)
            return Result.Failure<CreateOrderResponse>($"Payment error: {stripeResult.Error}");

        // 🔗 8. Save Stripe Session ID to order (for webhook verification)
        order.StripeSessionId = stripeResult.Value.SessionId;
        
        // 💾 9. Save Order + Transactions to database (Status = Pending)
        repo.Add(order);
        await repo.SaveChangesAsync(cancellationToken);
        
        // 🎫 10. Mark coupon as used (if applied)
        if (appliedCoupon != null)
        {
            repo.Add(new couponUser 
            { 
                couponId = appliedCoupon.couponId, 
                userId = buyerId, 
                UsedAt = DateTime.UtcNow 
            });
            await repo.SaveChangesAsync(cancellationToken);
        }

        // 🎯 11. Return checkout URL to frontend
        return Result.Success(new CreateOrderResponse(
            OrderId: orderId,
            CheckoutUrl: stripeResult.Value.CheckoutUrl,
            IsFree: false
        ));
    }
}