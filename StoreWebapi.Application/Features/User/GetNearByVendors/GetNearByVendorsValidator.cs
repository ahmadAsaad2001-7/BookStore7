using FluentValidation;

namespace StoreWebapi.Application.Features.User.GetNearByVendors;

public class GetNearByVendorsValidator :AbstractValidator<GetNearByVendorsQuery>
{
    public GetNearByVendorsValidator()
    {
        RuleFor(v => v.UserIpAddress).NotEmpty().WithMessage("UserIpAddress is required");
    }
}