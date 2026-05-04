namespace StoreWebapi.Application.Features.Admin.GenerateCoupons;

public class GenerateCouponResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal DiscountPercentage { get; set; }
}