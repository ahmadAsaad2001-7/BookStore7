using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Interfaces;
using StoreWebapi.Domain.Domain; 

namespace StoreWebapi.Application.Features.Books.Queries.GetBooks;

public class GetBooksHandler(IRepository repository) 
    : IRequestHandler<GetBooksQuery, Result<List<GetBooksResponse>>>
{
    public async Task<Result<List<GetBooksResponse>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
      
        var books = await repository.FindAll<Book>();

        
        var response = books.Select(book => new GetBooksResponse
        {
            Id = book.id,
            Name = book.name,
            Description = book.description,
            Author = book.author,
            Price = book.price,
            Isbn = book.isbn,
            ImageUrl = book.imageUrl,
            Rating = book.rating,
           
            Genres = book.genres?.Select(g => g.ToString()).ToList() ?? new List<string>() 
        }).ToList();
       return Result.Success(response);
    }
}