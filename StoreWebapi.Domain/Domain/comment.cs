namespace StoreWebapi.Domain.Domain;

public class comment
{
    public Guid id { get; set; }
    public string? text { get; set; }
    public DateTime? date { get; set; }
    public user ? user { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public Guid bookId { get; set; }
    public Book? book { get; set; } 
    
}