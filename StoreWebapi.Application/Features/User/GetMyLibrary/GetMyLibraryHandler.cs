
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using StoreWebapi.Application.Features.User.GetMyLibrary;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

public class GetMyLibraryHandler(
    IRepository repository,
    IHttpContextAccessor httpAccessor) 
    : IRequestHandler<GetMyLibraryQuery, Result<List<GetMyLibraryResponse>>>
{
    public async Task<Result<List<GetMyLibraryResponse>>> Handle(
        GetMyLibraryQuery request, 
        CancellationToken cancellationToken)
    {
        // 🔐 Step 1: Get the CURRENT authenticated user's ID (from cookie)
        var currentUserIdClaim = httpAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(currentUserIdClaim))
            return Result.Failure<List<GetMyLibraryResponse>>("Unauthorized");

        var currentUserId = Guid.Parse(currentUserIdClaim);
        

        Guid targetUserId;
        
        if (request.UserId.HasValue)
        {
      
            targetUserId = request.UserId.Value;
            
        
            var isOwner = targetUserId == currentUserId;
            var isAdmin = httpAccessor.HttpContext?.User.IsInRole("Admin") == true;
            
            if (!isOwner && !isAdmin)
                return Result.Failure<List<GetMyLibraryResponse>>("Forbidden: Insufficient permissions");
        }
        else
        {
            
            targetUserId = currentUserId;
        }

        var library = await repository.FindAll<UserBook>(
            ub => ub.UserId == targetUserId);

        var response = new List<GetMyLibraryResponse>();
        foreach (var entry in library)
        {
            var book = await repository.FindById<Book>(entry.BookId);
            if (book != null)
                response.Add(new GetMyLibraryResponse(
                    book.id, 
                    book.name, 
                    entry.PurchaseDate));
        }

        return Result.Success(response);
    }
}