using CLIMFinders.Application.DTOs;

namespace CLIMFinders.Application.Interfaces
{
    public interface IJwtTokenService
    {
        (string Token, DateTime Expiration) GenerateToken(LoginResponseDto user);
    }
}
