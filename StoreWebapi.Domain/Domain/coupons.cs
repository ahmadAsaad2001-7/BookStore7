using Microsoft.EntityFrameworkCore;

namespace StoreWebapi.Domain.Domain;
public class coupons
{
    
    public  Guid couponId { get; set; } 
    public string code { get; set; }
    public int quantity { get; set; }
    public DateTime  expiryDate { get; set; }
    public DateTime createDate { get; set; }
    public ICollection<couponUser>  couponUsers { get; set; }
    public decimal Discount_percentage { get; set; }
    
    public bool IsExpired => expiryDate < DateTime.UtcNow;
    

}