using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    [Authorize(Roles = "Business,Users")]
    public class SubscriptionRenewModel(ILogger<SubscriptionRenewModel> logger) : PageModel
    {
        private readonly ILogger<SubscriptionRenewModel> _logger = logger;

        public void OnGet()
        {
        }
    }
}
