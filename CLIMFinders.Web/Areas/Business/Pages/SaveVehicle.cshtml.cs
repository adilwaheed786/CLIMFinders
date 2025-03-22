using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Infrastructure.Repositories;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CLIMFinders.Web.Areas.Business.Pages
{ 
    [CustomAuthorize("Business", "SuperAdmin")]
    public class SaveVehicleModel(IVehicleService vehicleService, IAuthService authService, IEmailService emailService) : PageModel
    {
        private readonly IVehicleService vehicleService = vehicleService;
        private readonly IAuthService _authService = authService;
        private readonly IEmailService _emailService = emailService;
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
            else
            {
                var VehicelNotFoundExist = vehicleService.MatchVehicleNotFound(result.Name);
                if (VehicelNotFoundExist != null)
                {
                    var normaluser = _authService.GetUser(VehicelNotFoundExist.UserId);
                    string normalsubject = "Vehicle Has Been Found";
                    string normalmessage = $@"
            <p>Dear {normaluser.FullName},</p>
            <p>We have found a vehicle matching the details you provided:</p>
            <ul>
                <li><strong>VIN:</strong> {VehicelNotFoundExist.VIN}</li>
                <li><strong>Year:</strong> {VehicelNotFoundExist.Year}</li>
                <li><strong>Location:</strong> {VehicelNotFoundExist.LocationDetails}</li>
                <li><strong>Impound Fees:</strong> ${VehicelNotFoundExist.ImpoundFees}</li>
            </ul>";
                    _emailService.SendEmail(normaluser.Email, normalsubject, normalmessage, true);
                    _emailService.SendEmail(result.Email, normalsubject, normalmessage, true);
                }
            }
            return RedirectToPage("/ManageVehicles", new { area = "Business" });

        }
        [BindProperty]
        public VehicleDto Input { get; set; } = new();
    }
}
