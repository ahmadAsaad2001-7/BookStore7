// 📁 WebApi/Controllers/StripeWebhookController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe;

[ApiController]
[Route("api/webhooks/stripe")]
public class StripeWebhookController(ISender mediator, IConfiguration config) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];
        var webhookSecret = config["Stripe:WebhookSecret"];
        
        try
        {
            // 🔐 Verify webhook signature (critical for security)
            var stripeEvent = EventUtility.ConstructEvent(json, signature, webhookSecret);
            
            // 🎯 Only handle payment completion events
            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                
                if (session != null && Guid.TryParse(session.ClientReferenceId, out var orderId))
                {
                    // ✅ Update order status to Paid + add to library
                    var command = new ConfirmOrderPaymentCommand(orderId);
                    var result = await mediator.Send(command);
                    
                    if (result.IsFailure)
                    {
                        // Log error but return 200 to Stripe (they'll retry)
                        Console.WriteLine($"Webhook failed for order {orderId}: {result.Error}");
                        // Consider sending alert to admin
                    }
                }
            }
            
            // ✅ Always return 200 to Stripe (even if you ignore the event)
            return Ok();
            
        }
        catch (StripeException ex)
        {
            // ❌ Return 400 for signature failures (Stripe won't retry)
            Console.WriteLine($"Webhook signature verification failed: {ex.Message}");
            return BadRequest("Invalid signature");
        }
        catch (Exception ex)
        {
            // ❌ Return 500 for unexpected errors (Stripe will retry)
            Console.WriteLine($"Webhook processing error: {ex.Message}");
            return StatusCode(500, "Webhook processing failed");
        }
    }
}