using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class UserService(IHttpContextAccessor httpContextAccessor)  : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int GetUserId()
        {
            return Convert.ToInt32( _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        public int GetBusinessId()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.BusinessId)?.Value);
        }
        public string GeneratePassword(int length)
        { 
            return Guid.NewGuid().ToString("n")[..length].ToUpper();
        }
    }
} 