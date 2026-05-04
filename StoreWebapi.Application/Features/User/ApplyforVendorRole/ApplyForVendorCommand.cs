using MediatR;

namespace StoreWebapi.Application.Features.User.ApplyforVendorRole;

public record ApplyForVendorCommand(Guid Initiator) : IRequest<Result<Guid>>
{
    public string subject { get; } = "RoleChange:Vendor";
}