// 📁 Infrastructure/Services/StripePaymentService.cs

using Microsoft.Extensions.Configuration;
using StoreWebapi.Domain.Common;
using StoreWebapi.Domain.Domain;
using Stripe;
using Stripe.Checkout;

public class StripePaymentService : IPaymentService
{
    private readonly IConfiguration _config;
    
    public StripePaymentService(IConfiguration config)
    {
        _config = config;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    public async Task<Result<PaymentSessionResult>> CreateCheckoutSessionAsync(
        Order order, 
        List<transaction> transactions, 
        string successUrl, 
        string cancelUrl)
    {
        try
        {
            // 🛍️ Build line items for Stripe (one per transaction/book)
            var lineItems = new List<SessionLineItemOptions>();
            
            foreach (var t in transactions)
            {
                // Stripe expects amount in cents (USD)
                var amountInCents = (long)(t.amount * 100);
                
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = amountInCents,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = t.book?.name ?? $"Book #{t.bookId}",
                            Description = $"Purchase from vendor",
                            Metadata = new Dictionary<string, string>
                            {
                                { "bookId", t.bookId.ToString() },
                                { "vendorId", t.vendorId.ToString() }
                            }
                        }
                    },
                    Quantity = 1
                });
            }

            // 🎫 Create the Checkout Session
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment", // One-time payment (not subscription)
                
                // 🔗 Critical: Pass your order ID so webhook can find it later
                ClientReferenceId = order.Id.ToString(),
                
                // 🔄 Redirect URLs after payment
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                
                // 📦 Metadata for your own reference (not sent to user)
                Metadata = new Dictionary<string, string>
                {
                    { "orderId", order.Id.ToString() },
                    { "buyerId", order.BuyerId.ToString() }
                },
                
                // 🧾 Optional: Prefill user email if you have it
                // CustomerEmail = buyerEmail,
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Result.Success(new PaymentSessionResult(
                SessionId: session.Id,
                CheckoutUrl: session.Url
            ));
            
        }
        catch (StripeException ex)
        {
            return Result.Failure<PaymentSessionResult>($"Stripe error: {ex.StripeError?.Message ?? ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Failure<PaymentSessionResult>($"Payment service error: {ex.Message}");
        }
    }
}