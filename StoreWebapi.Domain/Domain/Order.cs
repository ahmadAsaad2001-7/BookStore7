namespace StoreWebapi.Domain.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public user Buyer { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } //
    public string StripeSessionId { get; set; } 
    public DateTime OrderDate { get; set; }
    public string Destination { get; set; }
    
  
    public ICollection<transaction> Transactions { get; set; }
}