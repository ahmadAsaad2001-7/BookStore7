namespace StoreWebapi.Application.Common;

public class StripeSessionResult
{
    public string SessionId { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
}