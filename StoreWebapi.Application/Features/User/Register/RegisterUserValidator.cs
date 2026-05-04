using System.Data;
using FluentValidation;

namespace StoreWebapi.Application.Features.User;

public class RegisterUserValidator :AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x=>x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(x=>x.Password).NotEmpty().MinimumLength(12).MaximumLength(35).WithMessage("Password is required");
        RuleFor(x=>x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x=>x.ImageUrl).NotEmpty().WithMessage("ImageUrl is required");
        RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is required");
        
    }
}