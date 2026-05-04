using MediatR;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;
using Stripe;

namespace StoreWebapi.Application.Features.Admin.GetAllCoupons;

public record GetAllCouponsQuery() :  IRequest<Result<List<GetAllCouponsResponse>>>;
public class  GetAllCouponsResponse
{
    public Guid CouponId { get; set; }
    public string Code { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime CreationDate { get; set; }
    public decimal  DiscountPercentage{ get; set; }
    public bool IsExpired { get; set; }
    
    
}
public class GetAllCouponsHandler(IRepository repo):IRequestHandler<GetAllCouponsQuery,Result<List<GetAllCouponsResponse>>>
{
    public async Task<Result<List<GetAllCouponsResponse>>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
    {
        var Coupons= await repo.FindAll<coupons>();
        var response = Coupons.Select(c=>new GetAllCouponsResponse
        {
            CouponId = c.couponId,
            Code = c.code,
            Quantity =  c.quantity,
            Expiration = c.expiryDate,
            CreationDate = c.createDate,
            DiscountPercentage = c.Discount_percentage,
            IsExpired =  c.IsExpired
        }).ToList();
        
        return Result.Success(response);
    }
}