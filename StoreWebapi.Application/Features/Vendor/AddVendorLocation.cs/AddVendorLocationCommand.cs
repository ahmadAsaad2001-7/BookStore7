using MediatR;

namespace StoreWebapi.Application.Features.Vendor.AddVendorLocation.cs;

public record AddVendorLocationCommand :IRequest<Result<AddVendorLocationResponse>>
{
     
    public string VendorIpAddress { get; init; }
}