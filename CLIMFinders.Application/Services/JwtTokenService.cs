using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Enums;
using CLIMFinders.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CLIMFinders.Application.Services
{
    public class JwtTokenService(IConfiguration config) : IJwtTokenService
    {
        private readonly IConfiguration _config = config;

        public (string Token, DateTime Expiration) GenerateToken(LoginResponseDto user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, Enum.GetName(typeof(RoleEnum), user.RoleId)),
                new(CustomClaimTypes.SubscriptionId, user.SubscriptionId),
                new(CustomClaimTypes.SessionId, user.SessionId),
                new(CustomClaimTypes.ActiveSubscription, user.IsActiveSubscription.ToString())
            };

            if (!string.IsNullOrEmpty(user.BusinessId))
            {
                claims.Add(new Claim(CustomClaimTypes.BusinessId, user.BusinessId));
                claims.Add(new Claim(CustomClaimTypes.SubRoleId, Enum.GetName(typeof(SubRoleEnum), user.SubRoleId)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpiryMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: expires, // Token expiration time
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
