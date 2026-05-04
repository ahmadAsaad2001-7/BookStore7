using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Application.Features.Books.SearchBooks;


public record GetBookByIdQuery(Guid Id) : IRequest<Result<GetBookByIdResponse>>;