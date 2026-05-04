using FluentValidation;

namespace StoreWebapi.Application.Features.Admin.GenerateCoupons;

public class GenerateCouponValidator :AbstractValidator<GenerateCouponCommand>
{
    public GenerateCouponValidator()
    {
        RuleFor(x=>x.code).NotEmpty().WithMessage("Code is required");
        RuleFor(x=>x.quantity).NotEmpty().WithMessage("Quantity is required");
        RuleFor(x=>x.Discount_percentage).NotEmpty().WithMessage("Discount percentage is required");
        RuleFor(x=>x.expiryDate).NotEmpty().WithMessage("Expiry Date is required");
        
    }
}