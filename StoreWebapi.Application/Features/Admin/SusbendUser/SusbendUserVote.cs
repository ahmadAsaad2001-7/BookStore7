// Application/Features/Admin/SuspendUser/SuspendUserCommand.cs
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

// ✅ Command (renamed from "Request" - this is a state change, so it's a Command)
public record SuspendUserCommand(
    Guid AdminId,        
    Guid TargetUserId,   
    string? Reason = null, 
    DateTime? Until = null 
) : IRequest<Result<SuspendUserResponse>>;


public class SuspendUserResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool IsSuspended { get; set; }      
    public DateTime? SuspendedAt { get; set; } 
    public DateTime? SuspendedUntil { get; set; } 
    public string? SuspensionReason { get; set; }
    public string Message { get; set; } = "User suspension updated successfully";
}


public class SuspendUserCommandValidator : AbstractValidator<SuspendUserCommand>
{
    public SuspendUserCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("Target user ID is required");
        
        RuleFor(x => x.AdminId)
            .NotEmpty().WithMessage("Admin ID is required")
            .NotEqual(x => x.TargetUserId).WithMessage("Admin cannot suspend themselves");
    }
}
public class SuspendUserHandler(
    UserManager<user> userManager,  
    IRepository repo,              
    ILogger<SuspendUserHandler> logger) 
    : IRequestHandler<SuspendUserCommand, Result<SuspendUserResponse>>
{
    public async Task<Result<SuspendUserResponse>> Handle(
        SuspendUserCommand request, 
        CancellationToken cancellationToken)
    {
        // 🔍 1. Validate target user exists
        var targetUser = await userManager.FindByIdAsync(request.TargetUserId.ToString());
        if (targetUser == null)
            return Result.Failure<SuspendUserResponse>("User not found");

        // 🚫 2. Prevent self-suspension & admin-on-admin (optional business rule)
        if (request.AdminId == request.TargetUserId)
            return Result.Failure<SuspendUserResponse>("Admin cannot suspend themselves");
            
        var adminUser = await userManager.FindByIdAsync(request.AdminId.ToString());
        if (adminUser != null && await userManager.IsInRoleAsync(adminUser, "ADMINISTRATOR") 
            && await userManager.IsInRoleAsync(targetUser, "ADMINISTRATOR"))
        {
            // Optional: Only super-admins can suspend other admins
            return Result.Failure<SuspendUserResponse>("Insufficient permissions to suspend another admin");
        }

        // ✅ 3. Update suspension state using UserManager (proper Identity way)
        targetUser.isSusbended = true; 
        targetUser.SuspendedAt = DateTime.UtcNow;
        targetUser.SuspensionReason = request.Reason;
        targetUser.SuspendsUntil = (DateTime)request.Until; 
        
        var updateResult = await userManager.UpdateAsync(targetUser);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            logger.LogError("Failed to suspend user {UserId}: {Errors}", request.TargetUserId, errors);
            return Result.Failure<SuspendUserResponse>($"Failed to update user: {errors}");
        }

        // 📝 4. Optional: Log audit trail
        logger.LogInformation("Admin {AdminId} suspended user {TargetUserId}. Reason: {Reason}", 
            request.AdminId, request.TargetUserId, request.Reason ?? "No reason provided");

        // ✅ 5. Return clean response for frontend
        var response = new SuspendUserResponse
        {
            UserId = targetUser.Id,
            UserName = targetUser.UserName ?? string.Empty,
            IsSuspended = true,
            SuspendedAt = targetUser.SuspendedAt,
            SuspendedUntil = targetUser.SuspendsUntil,
            SuspensionReason = targetUser.SuspensionReason
        };

        return Result.Success(response);
    }
}