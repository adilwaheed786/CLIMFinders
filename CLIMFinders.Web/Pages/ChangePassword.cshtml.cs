using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Domain.Entities;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    [Authorize]
    public class ChangePasswordModel(IAuthService authService) : PageModel
    {
        private readonly IAuthService authService = authService;

        public void OnGet()
        {

        }
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ModelStateHelper.AddGlobalErrors(ModelState);
                return Page();
            }
            var result = authService.ChangePassword(Input);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, result.Status);
                return Page();
            }
            ModelState.AddModelError(string.Empty, result.Status);

            return Page();
        }

        [BindProperty]
        public ChangePasswordDto Input { get; set; }
    }
}
