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
    public class ForgotPasswordModel(IAuthService authService) : PageModel
    {
        private readonly IAuthService _authService = authService;

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
            var respoonse = _authService.ResetPassword(Input);
            ModelState.AddModelError(string.Empty, respoonse.Status);
            return Page();
        }

        [BindProperty]
        public ForgotPasswordDto Input { get; set; }
    }
}
