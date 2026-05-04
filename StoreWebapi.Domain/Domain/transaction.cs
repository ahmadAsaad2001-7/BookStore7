using System.Reflection.Metadata;
using StoreWebapi.Domain.Domain.Enums;

namespace StoreWebapi.Domain.Domain;
public class transaction
{
    public Guid id { get; set; }
    public Guid userId  { get; set; }
    public user buyer { get; set; }
    public Guid vendorId { get; set; }
    public user vendor {get;set;}
    public Book book { get; set; }
    public Guid bookId { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    public decimal PaidAmount { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime TransactionDate { get; set; }
    public string destination { get; set; }
    public decimal amount { get; set; }
    public string currency { get; set; }
  
    
}