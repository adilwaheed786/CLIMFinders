using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CLIMFinders.Web.Areas.Business.Pages
{ 
    [CustomAuthorize("Business", "SuperAdmin")]
    public class SaveVehicleModel(IVehicleService vehicleService) : PageModel
    {
        private readonly IVehicleService vehicleService = vehicleService;
        public List<SelectListItem> VehicleColors { get; set; }
        public List<SelectListItem> VehicleMakes { get; set; }
        public List<SelectListItem> ModelYear { get; set; }
        public List<SelectListItem> VehicleModels { get; set; }
        public List<SelectListItem> StatusOptions { get; set; }

        public void OnGet(int? id)
        {
            if (id.HasValue)
            {
                Input = vehicleService.GetVehicle(id.Value);
            }
            BindDropdowns();
        }

        private void BindDropdowns()
        {
            VehicleMakes = vehicleService.GetVehicleMakes();
            VehicleColors = vehicleService.GetVehicleColors();
            ModelYear = vehicleService.PopulateYear();
            StatusOptions = vehicleService.StatusOptions();
            if (Input.Id > 0)
            {
                VehicleModels = vehicleService.GetVehicleModel(Input.MakeId);
            }
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelStateHelper.AddGlobalErrors(ModelState);
                BindDropdowns();
                return Page();
            }
            var result = vehicleService.SaveVehicle(Input);
            if (result.Id == -1)
            {
                BindDropdowns();
                ModelState.AddModelError(string.Empty, result.Status);
                return Page();
            }
            return RedirectToPage("/ManageVehicles", new { area = "Business" });

        }
        [BindProperty]
        public VehicleDto Input { get; set; } = new();
    }
}
