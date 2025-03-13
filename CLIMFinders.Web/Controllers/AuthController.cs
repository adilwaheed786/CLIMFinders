using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLIMFinders.Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IJwtTokenService jwtTokenService, IAuthService authService) : ControllerBase
    {
        private readonly IAuthService authService = authService;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        [HttpGet("testest")]
        public IActionResult LLL()
        {
            return Ok(new { token = "Done" });
        }
        [HttpPost("Authenticate")]
        [AllowAnonymous] 
        public IActionResult Login([FromBody] LoginDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { message = "Invalid login request." });
            }

            var loginObj = new LoginDto
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = authService.UserLogin(loginObj);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            // Generate JWT token
            var (token, expiration) = _jwtTokenService.GenerateToken(result);
            result.Token = token;

            // Set token in HttpOnly cookie
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,  // Ensure it's sent over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = expiration  // Ensure cookie expiration matches token expiration
            });

            return Ok(new
            {
                result,
                token,
                expiresAt = expiration
            });
        }
    }
}
