using MediatR;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;
using Stripe;

namespace StoreWebapi.Application.Features.User;

public record GetCouponByCodeQuery (string code)  : IRequest<Result<GetCouponByCodeResponse>>;

public class GetCouponByCodeResponse()
{
 
    
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Discount_percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
    
}
public class GetCouponByCodeQueryHandler(IRepository repo) 
    : IRequestHandler<GetCouponByCodeQuery, Result<GetCouponByCodeResponse>>
{
    public async Task<Result<GetCouponByCodeResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        // 1. البحث عن الكوبون في قاعدة البيانات
        // نفترض وجود Entity باسم Coupon
        var coupon = await repo.FindFirstOrDefault<coupons>(c => c.code == request.code);

        // 2. التحقق من وجود الكوبون
        if (coupon is null)
        {
            return Result.Failure<GetCouponByCodeResponse>("كود الخصم هذا غير موجود.");
        }

        // 3. التحقق من تاريخ الصلاحية
        if (coupon.expiryDate < DateTime.UtcNow)
        {
            return Result.Failure<GetCouponByCodeResponse>("عذراً، هذا الكوبون انتهت صلاحيته.");
        }

        // 4. التحقق من الكمية المتاحة
        if (coupon.quantity <= 0)
        {
            return Result.Failure<GetCouponByCodeResponse>("عذراً، هذا الكوبون استنفذ بالكامل.");
        }

        // 5. بناء الـ Response وإرجاع النتيجة بنجاح
        var response = new GetCouponByCodeResponse
        {
            Id = coupon.couponId,
            Code = coupon.code,
            Discount_percentage = coupon.Discount_percentage,
            ExpiryDate = coupon.expiryDate,
            Quantity = coupon.quantity
        };

        return Result.Success(response);
    }
}
