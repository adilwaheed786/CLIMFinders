using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CLIMFinders.Web.ServiceExtension
{
    public class JwtCookieMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;

        public async Task Invoke(HttpContext context) 
        {
            var token = context.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                AttachUserToContext(_configuration, context, token);
            }
            await _next(context);
        }

        private static void AttachUserToContext(IConfiguration config, HttpContext context, string? token)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = key
                }, out _);

                context.User = claimsPrincipal; // Attach user to context
            }
            catch (SecurityTokenException)
            {
                context.Response.Cookies.Delete("AuthToken"); // Invalid token, remove it
            }
        }

    }
}