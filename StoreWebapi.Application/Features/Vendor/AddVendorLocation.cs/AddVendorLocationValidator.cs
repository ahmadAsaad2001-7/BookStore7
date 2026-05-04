using FluentValidation;

namespace StoreWebapi.Application.Features.Vendor.AddVendorLocation.cs;

public class AddVendorLocationValidator : AbstractValidator<AddVendorLocationCommand>
{
    public AddVendorLocationValidator()
    {
        RuleFor(x => x.VendorIpAddress)
            .NotEmpty().WithMessage("IP Address is required.")
            .Must(BeAValidIp).WithMessage("Invalid IP Address format."); 
    }

    private bool BeAValidIp(string ip)
    {
        return System.Net.IPAddress.TryParse(ip, out _);
    }
}