// 📁 Application/Features/Orders/ConfirmOrderPayment/ConfirmOrderPaymentHandler.cs
using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;
using StoreWebapi.Domain.Interfaces;

public record ConfirmOrderPaymentCommand(Guid OrderId) : IRequest<Result>;

public class ConfirmOrderPaymentHandler(IRepository repo) 
    : IRequestHandler<ConfirmOrderPaymentCommand, Result>
{
    public async Task<Result> Handle(ConfirmOrderPaymentCommand request, CancellationToken ct)
    {
        // 🔍 Find order
        var order = await repo.FindById<Order>(request.OrderId, ct);
        if (order == null) 
            return Result.Failure("Order not found");
        
        // ✅ Idempotency: Already processed?
        if (order.Status == PaymentStatus.Paid.ToString())
            return Result.Success();
        
        // 🔒 Security: Only update if Stripe session matches (prevent fraud)
        // (Optional but recommended: verify session with Stripe API)
        
        // 💰 Update all transactions to Paid
        var transactions = await repo.FindAll<transaction>(
            t => t.OrderId == request.OrderId, ct);
        
        foreach (var t in transactions)
        {
            t.Status = PaymentStatus.Paid;
            t.DeliveryDate = DateTime.UtcNow; // Digital delivery = instant
            
            // 📚 Add book to user's library
            repo.Add(new UserBook
            {
                UserId = t.userId,
                BookId = t.bookId,
                PurchaseDate = DateTime.UtcNow
            });
        }
        
        // 📦 Update order status
        order.Status = PaymentStatus.Paid.ToString();
        
        // 💾 Persist all changes
        await repo.SaveChangesAsync(ct);
        
        // 🎉 Optional: Send confirmation email, notify vendor, etc.
        
        return Result.Success();
    }
}