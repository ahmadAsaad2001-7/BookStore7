namespace StoreWebapi.Domain.Domain;
public class couponUser
{
    public Guid couponId { get; set; }
    public coupons coupon { get; set; }
    public user user { get; set; }
    public Guid userId { get; set; }
    public DateTime UsedAt { get; set; }
    
    
}