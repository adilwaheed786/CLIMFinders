using CLIMFinders.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.Interfaces
{
    public interface IAuthService
    {
        LoginResponseDto UserLogin(LoginDto loginDto);
        ResponseDto ChangePassword(ChangePasswordDto dto);
        ResponseDto ResetPassword(ForgotPasswordDto dto); 
    }
}
