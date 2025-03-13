using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{
    public class ActivateAccountModel(IRegisterService registerService) : PageModel
    {
        private readonly IRegisterService _registerService = registerService;

        [BindProperty(SupportsGet = true)]
        public string code { get; set; }
        public void OnGet()
        {
            Message = !string.IsNullOrEmpty(code) && _registerService.ActivateAccount(code) ? "Account is successfully created!": "Invalid or expired token!";
        }
        [BindProperty]
        public string Message { get; set; }
    }
}
