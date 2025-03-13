using CLIMFinders.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CLIMFinders.StripeProcess.Interfaces;
using Stripe;

namespace CLIMFinders.Web.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController(ISubscriptionPlanServices services) : ControllerBase
    {
        private readonly ISubscriptionPlanServices _services = services;

        [HttpPost("PostSubscription")]
        public IActionResult PostSub([FromBody] SubscriptionRequest plan)
        {
            string sessionUrl = _services.SubscripePlan(plan);
            // Return the session URL for the redirect 
            return new JsonResult(new { sessionUrl });
        }
        [HttpPost("RenewSubscription")]
        public IActionResult RenewSubscription([FromBody] RenewRequest renew) 
        {
            string sessionUrl = _services.CreateRenewalCheckoutSession(renew.SessionId, renew.PriceId, renew.UserId);
            // Return the session URL for the redirect
            return new JsonResult(new { sessionUrl });
        }
        [HttpPost("CancelSubscription")]
        public IActionResult CancelSubscription([FromBody] CancelRequest renew)
        {
            var response = _services.CancelSubscription(renew.SubscriptionId);
            // Return the session URL for the redirect
            return new JsonResult(new { response }); 
        }
    }
}