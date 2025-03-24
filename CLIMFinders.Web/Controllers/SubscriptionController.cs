using CLIMFinders.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CLIMFinders.StripeProcess.Interfaces;
using Stripe;
using CLIMFinders.Application.Interfaces;

namespace CLIMFinders.Web.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController(ISubscriptionPlanServices services, IVehicleService vehicleService) : ControllerBase
    {
        private readonly ISubscriptionPlanServices _services = services;
        private readonly IVehicleService _vehicleService = vehicleService;

        [HttpPost("PostSubscription")]
        public IActionResult PostSub([FromBody] SubscriptionRequest plan)
        {
            string sessionUrl = _services.SubscripePlan(plan);
            // Return the session URL for the redirect 
            return new JsonResult(new { sessionUrl });
        }
        [HttpPost("ImpoundFeePayment")]
        public IActionResult PostPayment([FromBody] PayImpoundFeesRequest request)
        {
            if (request.vehicleVIN != null && request.vehicleVIN.Length > 0) {
                var vehicle = _vehicleService.GetVehicleByVIN(request.vehicleVIN);
                string sessionUrl = _services.ImpoundFeePayment(vehicle);
                return new JsonResult(new { sessionUrl });
            }
            return new JsonResult("N");
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