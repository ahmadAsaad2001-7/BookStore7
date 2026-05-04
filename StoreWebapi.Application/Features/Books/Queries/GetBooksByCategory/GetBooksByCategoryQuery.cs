using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Application.Features.Books.Queries.GetBooksByCategory;

public record GetBooksByCategoryQuery(List<Genres> GenresList) :IRequest<GetBooksByCategoryResponse>, IRequest<Result<List<GetBooksByCategoryResponse>>>;