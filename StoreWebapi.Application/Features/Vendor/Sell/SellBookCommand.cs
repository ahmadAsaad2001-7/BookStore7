using MediatR;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Application.Features.Vendor.Sell;

public class SellBookCommand : IRequest<Result<SellBookResponse>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Isbn { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public List<Genres> Genres { get; init; } = new();
}