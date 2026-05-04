using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;
using Stripe;

namespace StoreWebapi.Application.Features.Admin.GenerateCoupons;

public class GenerateCouponHandler :IRequestHandler<GenerateCouponCommand,Result<GenerateCouponResponse>>
{
    private readonly IRepository _repository;
 
    public GenerateCouponHandler(IRepository repository, UserManager<user> userManager)
    {
        _repository = repository;

    }
    public async Task<Result<GenerateCouponResponse>> Handle(GenerateCouponCommand request, CancellationToken cancellationToken)
    {
        var exists= await _repository.AnyAsync<coupons>(c=>c.code==request.code);
        if (exists)
            return Result.Failure<GenerateCouponResponse>("A coupon with this code already exists.");
        var coupon = new coupons
        {
            couponId = Guid.NewGuid(),
            code = request.code,
            expiryDate = request.expiryDate,
            Discount_percentage = request.Discount_percentage,
            quantity = request.quantity,
        };
        _repository.Add(coupon);
        await _repository.SaveChangesAsync(cancellationToken);
        var response = new GenerateCouponResponse
        {
            Id = coupon.couponId,
            Code = coupon.code,
            Quantity = coupon.quantity,
            ExpiryDate = coupon.expiryDate,
            DiscountPercentage = coupon.Discount_percentage
        };
       
        return Result<GenerateCouponResponse>.Success(response);
    }
}