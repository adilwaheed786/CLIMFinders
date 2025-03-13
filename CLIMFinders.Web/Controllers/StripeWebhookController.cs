using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace CLIMFinders.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebhookController(IConfiguration configuration, ILogger<StripeWebhookController> logger) : ControllerBase
    {
        private readonly ILogger<StripeWebhookController> _logger = logger;
        private readonly string _webhookSecret = configuration["Stripe:WebhookSecret"];

        [HttpPost]
        public async Task<IActionResult> StripeWebhook() 
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signature, _webhookSecret);
                _logger.LogInformation($"Webhook received: {stripeEvent.Type}");

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    string customerId = session.CustomerId;
                    _logger.LogInformation($"Subscription successful for Customer: {customerId}");
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError($"Webhook error: {e.Message}");
                return BadRequest();
            }
        }
    }

}
