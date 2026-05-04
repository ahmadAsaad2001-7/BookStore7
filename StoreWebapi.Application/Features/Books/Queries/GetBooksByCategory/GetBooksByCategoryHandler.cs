using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Books.Queries.GetBooksByCategory;

// 1. FIX: Changed the return type to Result<List<...>> because a category search returns multiple books
public class GetBooksByCategoryHandler(IRepository repository) 
    : IRequestHandler<GetBooksByCategoryQuery, Result<List<GetBooksByCategoryResponse>>>
{
    public async Task<Result<List<GetBooksByCategoryResponse>>> Handle(GetBooksByCategoryQuery request, CancellationToken cancellationToken)
    {
        
        var books = await repository.FindAll<Book>(b => 
            b.genres.Any(g => request.GenresList.Contains(g)));

     
        if (books == null || !books.Any())
        {
            return Result.Success(new List<GetBooksByCategoryResponse>());
        }

     
        var response = books.Select(book => new GetBooksByCategoryResponse
        {
            Id = book.id,
            Name = book.name,
            Author = book.author,
            Description = book.description,
            Price = book.price,
            Isbn = book.isbn,
            ImageUrl = book.imageUrl,
            Rating = book.rating,
            Genres = book.genres.Select(g => g.ToString()).ToList() 
        }).ToList();
        
        return Result.Success(response);
    }
}