using FluentValidation;

namespace StoreWebapi.Application.Features.User;

public class LoginUserValidator :AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(l=>l.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(l=>l.Password).NotEmpty().WithMessage("Password is required");
    }
}