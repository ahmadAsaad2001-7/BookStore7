using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Books.Commands.Update;

public class UpdateBookHandler(IRepository repo) : IRequestHandler<UpdateBookCommand, Result<UpdateBookResponse>>
{
    public async Task<Result<UpdateBookResponse>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var updatedBook = await repo.FindById<Book>(request.Id);
        if (updatedBook == null)
            return Result.Failure<UpdateBookResponse>("there is no book for this Id");

        updatedBook.name = request.name;
        updatedBook.description = request.description;
        updatedBook.author = request.author;
        updatedBook.price = request.price;
        updatedBook.isbn = request.isbn;
        updatedBook.imageUrl = request.imageUrl;
        updatedBook.genres = request.genres;
        updatedBook.Version++;

        repo.Update(updatedBook);
        await repo.SaveChangesAsync(cancellationToken);

        var response = new UpdateBookResponse
        {
            name = updatedBook.name,
            description = updatedBook.description,
            author = updatedBook.author,
            price = updatedBook.price,
            isbn = updatedBook.isbn,
            imageUrl = updatedBook.imageUrl,
            genres = updatedBook.genres
        };

        return Result.Success(response);
    }
}
