using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    public class ContactUsModel(ILogger<ContactUsModel> logger) : PageModel
    {
        private readonly ILogger<ContactUsModel> _logger = logger;

        public void OnGet()
        {

        }
    }

}
