using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    [CustomAuthorize("Users")]
    public class SearchModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
