using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Application.Features.User;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    private readonly SignInManager<user> _signInManager;
    private readonly UserManager<user> _userManager;


    public LoginUserHandler(SignInManager<user> signInManager, UserManager<user> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }


    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
       
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
            return Result<LoginUserResponse>.Failure<LoginUserResponse>("Invalid email or password.");

        if (user.isSusbended && user.SuspendsUntil.HasValue && user.SuspendsUntil.Value > DateTime.UtcNow)
        {
            return Result<LoginUserResponse>.Failure<LoginUserResponse>(
                $"Your account is suspended until {user.SuspendsUntil.Value:g}. Reason: {user.SuspensionReason ?? "No reason provided."}");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
    
        if (result.Succeeded)
        {
            var response = new LoginUserResponse
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                UserName = user.UserName
            };
            return Result<LoginUserResponse>.Success(response);
        }

        return Result<LoginUserResponse>.Failure<LoginUserResponse>("Invalid email or password.");
    }
}