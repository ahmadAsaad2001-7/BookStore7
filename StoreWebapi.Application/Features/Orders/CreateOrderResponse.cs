
public record CreateOrderResponse(
    Guid OrderId,          
    string CheckoutUrl,    
    bool IsFree            
);