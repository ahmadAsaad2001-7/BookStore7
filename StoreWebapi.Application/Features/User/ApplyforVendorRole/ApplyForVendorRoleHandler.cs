using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User.ApplyforVendorRole;

public class ApplyForVendorRoleHandler :IRequestHandler<ApplyForVendorCommand,Result<Guid>>
{
    private readonly UserManager<user> _userManager;
    private readonly IRepository _repository;
    public ApplyForVendorRoleHandler(UserManager<user> userManager, IRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }
    public async Task<Result<Guid>> Handle(ApplyForVendorCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByIdAsync(request.Initiator.ToString());
        if (existingUser == null)
            return Result.Failure<Guid>("User not found.");
        var roles = await _userManager.GetRolesAsync(existingUser);
        if (roles.Contains("Vendor"))
            return Result.Failure<Guid>("User is already a vendor.");
        var pendingVote = new vote
        {
            Id = Guid.NewGuid(),
            InitiatorId = request.Initiator,
            subject = request.subject,
            Approval = 0,
            disApprove = 0,
            IsResolved = false, 
            createDate = DateTime.UtcNow,
            expiryDate = DateTime.UtcNow.AddDays(14) 
        };
        _repository.Add(pendingVote);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success(pendingVote.Id);
    }
}