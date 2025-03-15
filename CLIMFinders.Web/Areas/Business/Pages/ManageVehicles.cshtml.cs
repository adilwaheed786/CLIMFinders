using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Areas.Business.Pages
{
    [CustomAuthorize("Business", "SuperAdmin")]
    public class ManageVehiclesModel(IVehicleService vehicleService) : PageModel
    {
        private readonly IVehicleService vehicleService = vehicleService;
         
        public void OnGet()
        {
            Input = vehicleService.GetVehicles();
        }
        public IActionResult OnGetDelete(int id)
        {
            vehicleService.DeleteVehicle(id);
            return RedirectToPage("/ManageVehicles", new { area = "Business" });
        }
        [BindProperty]
        public List<VehicleListDto> Input { get; set; } = new();
    }
}
