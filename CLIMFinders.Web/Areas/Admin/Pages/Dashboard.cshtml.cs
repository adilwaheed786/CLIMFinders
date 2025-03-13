using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Areas.Admin.Pages
{
    [CustomAuthorize("SuperAdmin")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
