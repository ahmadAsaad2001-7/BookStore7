using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Application.Features.Books.Commands.Update;

public class UpdateBookResponse
{
    
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public string author { get; set; } = string.Empty;
    public decimal price { get; set; }
    public string isbn { get; set; } = string.Empty;
    public string imageUrl { get; set; } = string.Empty;
    public List<Genres> genres { get; set; } = new();
}