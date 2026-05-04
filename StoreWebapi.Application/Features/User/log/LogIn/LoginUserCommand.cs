using MediatR;
using StoreWebapi.Application.Common;

namespace StoreWebapi.Application.Features.User;

public record LoginUserCommand :IRequest<Result<LoginUserResponse>>
{
   public string Password { get; set; } = string.Empty;
   public  string Email { get; set; } = string.Empty;
   public bool RememberMe { get; set; } = false;
    
}