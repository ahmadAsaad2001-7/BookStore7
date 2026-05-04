using MediatR;
using StoreWebapi.Application.Common;

namespace StoreWebapi.Application.Features.Books.Queries.GetBooks;

public record GetBooksQuery() : IRequest<Result<List<GetBooksResponse>>>;
