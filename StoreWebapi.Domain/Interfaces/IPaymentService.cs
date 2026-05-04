
using StoreWebapi.Domain.Domain;

public interface IPaymentService
{
    
    Task<Result<PaymentSessionResult>> CreateCheckoutSessionAsync(
        Order order, 
        List<transaction> transactions, 
        string successUrl, 
        string cancelUrl);
}

public record PaymentSessionResult(
    string SessionId,   
    string CheckoutUrl  
);