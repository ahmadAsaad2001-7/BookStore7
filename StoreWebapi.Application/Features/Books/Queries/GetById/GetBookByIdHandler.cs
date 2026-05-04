using MediatR;
using Microsoft.Extensions.Logging;
using StoreWebapi.Application.Common;
using StoreWebapi.Application.Features.Books.SearchBooks;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Books.Queries;

public class GetBookByIdHandler(IRepository repository) 
    : IRequestHandler<GetBookByIdQuery, Result<GetBookByIdResponse>>
{
    public async Task<Result<GetBookByIdResponse>> Handle(GetBookByIdQuery request, CancellationToken ct)
    {
       
            var book = await repository.FindById<Book>(request.Id);

            if (book is null)
            {
                return Result.Failure<GetBookByIdResponse>("Book with the specified ID was not found.");
            }


            var response = new GetBookByIdResponse
            {
                Id = book.id,
                Name = book.name,
                Description = book.description,
                Author = book.author,
                Price = book.price,
                Isbn = book.isbn,
                ImageUrl = book.imageUrl,
                Genres = book.genres?.Select(genre => genre.ToString()).ToList() ?? new List<string>(),
                Rating = book.rating,
            };


            return Result.Success(response);
      
     
        
        
    }
}