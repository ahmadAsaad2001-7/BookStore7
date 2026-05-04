namespace StoreWebapi.Domain.Domain;

public class UserBook
{
    
    public Guid UserId { get; set; }
    public user User { get; set; } = null!;

 
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

   
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}