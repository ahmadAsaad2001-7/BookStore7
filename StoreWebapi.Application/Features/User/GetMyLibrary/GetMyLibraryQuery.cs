using MediatR;

namespace StoreWebapi.Application.Features.User.GetMyLibrary;

public record GetMyLibraryQuery(Guid? UserId= null) : IRequest<Result<List<GetMyLibraryResponse>>>
{
   
    
    
}