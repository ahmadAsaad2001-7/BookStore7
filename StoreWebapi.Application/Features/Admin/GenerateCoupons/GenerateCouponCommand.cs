using MediatR;

namespace StoreWebapi.Application.Features.Admin.GenerateCoupons;

public record GenerateCouponCommand() : IRequest<Result<GenerateCouponResponse>>
{
    public string code { get; set; }
    public int quantity { get; set; }
    public DateTime  expiryDate { get; set; }
    public decimal Discount_percentage { get; set; }
}