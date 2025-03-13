using AutoMapper;
using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Enums;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Domain.Entities;
using CLIMFinders.StripeProcess.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class AuthService(IUnitOfWork unitOfWork, IHashManager hashManager, IMapper mapper, IUserService userService,
        IEmailService emailService, IEmailHelperUtils emailHelper, IConfiguration config, ISubscriptionPlanServices subscription) : IAuthService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IHashManager _hashManager = hashManager;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        private readonly IEmailHelperUtils _emailHelper = emailHelper;
        private readonly IConfiguration _config = config;
        private readonly ISubscriptionPlanServices _subscription = subscription;

        public LoginResponseDto UserLogin(LoginDto loginDto)
        {
            var response = new LoginResponseDto();
            try
            {
                var repository = unitOfWork.GetRepository<User>();
                var entity = repository.GetAllInclude(navigationProperties: u => u.Businesses).FirstOrDefault(e => e.Email == loginDto.Email && e.IsDeleted == false && e.IsConfirmed == true);

                if (entity == null || !_hashManager.VerifyPassword(loginDto.Password, entity.PasswordHash, entity.PasswordSalt))
                {
                    response.Id = 0;
                    response.UIMessage = "Invalid username or password.";
                }
                else
                {
                    response = _mapper.Map<LoginResponseDto>(entity);
                    response.IsActiveSubscription = entity.RoleId == (int)RoleEnum.SuperAdmin ? true : _subscription.IsSubscriptionActive(entity.SubscriptionId);
                }
                return response;
            }
            catch
            {
                response.Id = -1;
                response.UIMessage = "An error has been occurred on Login Attempt. Try after sometime.";
                throw;
            }
        }
        public ResponseDto ChangePassword(ChangePasswordDto dto)
        {
            var response = new ResponseDto();
            try
            {
                var repository = unitOfWork.GetRepository<User>();
                var entity = repository.GetById(_userService.GetUserId());

                if (entity == null || !_hashManager.VerifyPassword(dto.OldPassword, entity.PasswordHash, entity.PasswordSalt))
                {
                    response.Id = -1;
                    response.Status = "Current password is incorrect.";
                }
                else
                {
                    var newSalt = _hashManager.GenerateSalt();
                    entity.PasswordHash = _hashManager.HashPassword(dto.NewPassword, newSalt);
                    entity.PasswordSalt = newSalt;
                    entity.ModifiedById = entity.Id;
                    entity.ModifiedOn = DateTime.Now;
                    repository.Update(entity);
                    repository.Save();
                    response.Id = entity.Id;
                    response.RoleId = entity.RoleId;
                    response.Email = entity.Email;
                    response.Name = entity.FullName;
                    response.Status = "Your password has been changed successfully.";

                    SendEmailNotification(dto.NewPassword, entity);
                }
            }
            catch
            {
                response.Id = -1;
                response.Status = "An unexpected error occurred";
                throw;
            }
            return response;
        }



        public ResponseDto ResetPassword(ForgotPasswordDto dto)
        {
            var response = new ResponseDto();
            try
            {
                var repository = unitOfWork.GetRepository<User>();
                var entity = repository.FirstOrDefault(e => e.Email == dto.Email);
                if (entity == null)
                {
                    response.Id = -1;
                    response.Status = "Email does not exist in our system.";
                }
                else
                {
                    var newSalt = _hashManager.GenerateSalt();
                    entity.PasswordHash = _hashManager.HashPassword(dto.NewPassword, newSalt);
                    entity.PasswordSalt = newSalt;
                    entity.ModifiedById = entity.Id;
                    entity.ModifiedOn = DateTime.Now;
                    repository.Update(entity);
                    repository.Save();
                    response.Id = entity.Id;
                    response.Status = "Password reset successfully.";

                    SendEmailNotification(dto.NewPassword, entity);
                }

            }
            catch
            {
                response.Id = -1;
                response.Status = "An unexpected error occurred";
                throw;
            }
            return response;
        }
        private void SendEmailNotification(string NewPassword, User entity)
        {
            EmailContent emailContent = new()
            {
                BaseUrl = _config["JwtSettings:Issuer"],
                Name = entity.FullName,
                Email = entity.Email,
                ClickLink = "/Login",
                CopyRightYear = DateTime.Now.Year.ToString(),
                LogoLink = "/images/logo.png",
                OtherText = NewPassword
            };
            var ContentToFill = _emailHelper.FillEmailContents(emailContent, "resetpassword", entity.FullName);
            _emailService.SendEmail(entity.Email, "Password reset successfully", ContentToFill, true);
        }
    }
}
