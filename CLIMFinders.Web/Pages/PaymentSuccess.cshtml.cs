using CLIMFinders.StripeProcess.Interfaces;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    public class PaymentSuccessModel(ISubscriptionPlanServices services) : PageModel
    {
        private readonly ISubscriptionPlanServices _services = services;

        [BindProperty(SupportsGet = true)]
        public string Session_Id { get; set; }
        public void OnGet()
        {
            if (!string.IsNullOrEmpty(Session_Id))
            {
                _services.SendInvoiceOnPaymentSuccess(Session_Id, 0);
            }
               // _services.SendInvoiceOnSubscriptionSuccess(Session_Id, For_Id);
        }
    }
}
