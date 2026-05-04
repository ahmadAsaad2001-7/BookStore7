using MediatR;
using StoreWebapi.Application.Common;

namespace StoreWebapi.Application.Features.Books.Commands.Delete;

public record DeleteBookCommand(Guid Id) : IRequest<Result<Guid>>;
