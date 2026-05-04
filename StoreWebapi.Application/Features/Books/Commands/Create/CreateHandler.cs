using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Books.Commands.Create;


public class CreateHandler(IRepository repo) : IRequestHandler<CreateCommand, Result<Guid>> 
{
    public async Task<Result<Guid>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var Book = new Book
        {
            id = Guid.NewGuid(),
            name = request.name,
            description = request.description,
            author = request.author,
            price = request.price,
            isbn = request.isbn,
            imageUrl = request.imageUrl,
            genres = request.genres,
            Version = 1,      
            rating = 0       
        };

        repo.Add(Book);
        
  
        await repo.SaveChangesAsync(cancellationToken); 
        
        return Result<Guid>.Success(Book.id);
    }
}