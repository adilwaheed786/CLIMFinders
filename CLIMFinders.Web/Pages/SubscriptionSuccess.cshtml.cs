using CLIMFinders.StripeProcess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    public class SubscriptionSuccessModel(ISubscriptionPlanServices services) : PageModel
    {
        private readonly ISubscriptionPlanServices _services = services;

        [BindProperty(SupportsGet = true)]
        public string Session_Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int For_Id { get; set; }
        public void OnGet()
        {
            if (!string.IsNullOrEmpty(Session_Id))
                _services.SendInvoiceOnSubscriptionSuccess(Session_Id, For_Id);
        }
    }
}