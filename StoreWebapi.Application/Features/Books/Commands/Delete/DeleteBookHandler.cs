using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Books.Commands.Delete;

public class DeleteBookHandler(IRepository repo) :IRequestHandler<DeleteBookCommand,Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var bookToDelete = await repo.FindById<Book>(request.Id);

        if (bookToDelete is null)
        {
            return Result.Failure<Guid>("Book with the specified ID was not found.");
        }

        repo.Remove(bookToDelete);
        await repo.SaveChangesAsync(cancellationToken); 

        return Result.Success(bookToDelete.id);
    }
}
