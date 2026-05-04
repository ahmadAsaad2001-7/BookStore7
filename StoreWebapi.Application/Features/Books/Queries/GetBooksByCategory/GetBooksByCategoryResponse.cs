namespace StoreWebapi.Application.Features.Books.Queries.GetBooksByCategory;

public class GetBooksByCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new(); 
    public double Rating { get; set; }
}