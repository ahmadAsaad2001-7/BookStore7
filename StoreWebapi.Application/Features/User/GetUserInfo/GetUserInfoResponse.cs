using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Application.Features.User.GetUserInfo;

public class GetUserInfoResponse
{
    public Guid  Id { get; set; }
    public string imageUrl { get; set; }
    public string UserName { get; set; }
    public bool isSusbended { get; set; }
    public string? cellId { get; set; }
    
}