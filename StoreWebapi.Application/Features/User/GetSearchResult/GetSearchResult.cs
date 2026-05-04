using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User.GetSearchResult;

public record GetSearchResultQuery(string content) : IRequest<Result<GetSearchResultResponse>>;
public class GetSearchResultResponse
{
    public List<UserSearchDto> Users { get; set; } = new();
    public List<BookSearchDto> Books { get; set; } = new();
}
public class GetSearchResultHandler : IRequestHandler<GetSearchResultQuery, Result<GetSearchResultResponse>>
{
    private readonly UserManager<user> _userManager;
    private readonly IRepository _repo; 

    public GetSearchResultHandler(UserManager<user> userManager, IRepository repo)
    {
        _userManager = userManager;
        _repo = repo;
    }

    public async Task<Result<GetSearchResultResponse>> Handle(GetSearchResultQuery request, CancellationToken cancellationToken)
    {
        var response = new GetSearchResultResponse();
        var query = request.content?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(query))
            return Result.Success(response);

        if (query.StartsWith("u/", StringComparison.OrdinalIgnoreCase))
        {
            var searchTerm = query.Substring(2).Trim();

            var users = await _userManager.Users
                .Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm))
                .Take(10) 
                .Select(u => new UserSearchDto(
                    u.Id,
                    u.UserName,
                    u.imageUrl,
                    u.VendorTransactions.Any() 
                ))
                .ToListAsync(cancellationToken);

            response.Users = users;
        }
        else
        {
            var books = await _repo.GetQueryable<Book>() 
                .Where(b => b.name.Contains(query)|| b.author.Contains(query))
                .Take(10)
                .Select(b => new BookSearchDto(
                    b.id,
                    b.name,
                    b.author,
                    b.imageUrl, 
                    b.price
                ))
                .ToListAsync(cancellationToken);

            response.Books = books;
        }

        return Result.Success(response);
    }
}
public record UserSearchDto(Guid Id, string UserName, string ImageUrl, bool IsVendor);
public record BookSearchDto(Guid Id, string Title, string Author, string CoverUrl, decimal Price);