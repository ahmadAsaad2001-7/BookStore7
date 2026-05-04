using MediatR;
using StoreWebapi.Application.Features.User.GetMyLibrary;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Admin.GetAllUsers;

public class GetAllUsers
{
    public record GetAllUsersQuery() : IRequest<Result<List<GetAllUsersResponse>>>;

    public class GetAllUsersResponse()
    {
      public Guid UserId { get; set; }
        public string imageUrl { get; set; }
        public string UserName { get; set; }
        public bool isSusbended { get; set; }
        public string? cellId { get; set; }
    }
    
    public class GetAllUsersHandler(IRepository repo) : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUsersResponse>>>
    {
        public async Task<Result<List<GetAllUsersResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await repo.FindAll<user>();
            var response = users.Select(u => new GetAllUsersResponse
            {
            UserId =  u.Id,
            imageUrl = u.imageUrl,
            UserName = u.UserName,
            isSusbended = u.isSusbended,
            cellId = u.cellId
            
            }).ToList();
            return Result.Success(response);

        }
    }
}