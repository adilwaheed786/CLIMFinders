using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Enums;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{ 
    public class LoginModel(IJwtTokenService jwtTokenService, IAuthService authService) : PageModel
    {
        private readonly IAuthService authService = authService;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

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

            var result = authService.UserLogin(Input);

            if (result.Id <= 0)
            {
                ModelState.AddModelError(string.Empty, result.UIMessage);
                return Page();
            }

            // Generate JWT token
            var (token, expiration) = _jwtTokenService.GenerateToken(result);
            result.Token = token;

            // Set token in HttpOnly cookie
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensures the token is only sent over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = expiration // Sync cookie expiration with token expiration
            });

            // Redirect based on user role
            return result.RoleId switch
            {
                (int)RoleEnum.Business => RedirectToPage("/ManageVehicles", new { area = "Business" }),
                (int)RoleEnum.Users => RedirectToPage("/Search"),
                //(int)SubRoleEnum.Impound or (int)SubRoleEnum.Tow => RedirectToPage("/ManageVehicles", new { area = "Business" }),
                _ => RedirectToPage("/Index")
            };
        }

        [BindProperty]
        public LoginDto Input { get; set; }  
    }
}
