using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using StoreWebapi.Application.Common;

namespace StoreWebapi.Application.Features.User;

public record RegisterUserCommand :IRequest<Result<Guid>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;  
    public string? Name { get; init; }                     
    public string? ImageUrl { get; init; }

    
}