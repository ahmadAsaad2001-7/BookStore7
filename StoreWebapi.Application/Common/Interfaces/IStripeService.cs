using Stripe;
using Stripe.Checkout;

namespace StoreWebapi.Application.Common.Interfaces;

public interface IStripeService
{
    Task<Session> CreateCheckoutSessionAsync(SessionCreateOptions options, CancellationToken ct);
    Task<Session> GetSessionAsync(string sessionId, CancellationToken ct);
    Task<Event> ConstructWebhookEventAsync(string json, string stripeSignature, string webhookSecret);
}
