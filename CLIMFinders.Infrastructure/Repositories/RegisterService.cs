using AutoMapper;
using Azure;
using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Enums;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Domain.Entities;
using CLIMFinders.StripeProcess.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class RegisterService(IUnitOfWork unitOfWork, IHashManager hashManager, IMapper mapper, IUserService userService,
        IEmailService emailService, IEmailHelperUtils emailHelper, IConfiguration config, Lazy<ISubscriptionPlanServices> subscription) : IRegisterService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IHashManager _hashManager = hashManager;
        private readonly IMapper _mapper = mapper;
        private readonly IUserService _userService = userService;
        private readonly IEmailService _emailService = emailService;
        private readonly IEmailHelperUtils _emailHelper = emailHelper;
        private readonly IConfiguration _config = config;
        private readonly Lazy<ISubscriptionPlanServices> _subscription = subscription;

        public ResponseDto SaveUser(PersonInfoDto dto, int RoleID, int? SubRoleId = null, SubscriptionDto? subscription = null)
        {
            var response = new ResponseDto();
            try
            {
                if (IsUserExists(dto.Email))
                {
                    response.Id = -1;
                    response.Status = "Email already exists";
                }
                else
                {
                    string password = _userService.GeneratePassword(8);
                    string ConfirmationCode = Guid.NewGuid().ToString();
                    var Salt = _hashManager.GenerateSalt();
                    var hashedPassword = _hashManager.HashPassword(password, Salt);
                    var mappedRequest = new User()
                    {
                        Email = dto.Email,
                        FullName = dto.Name,
                        RoleId = RoleID,
                        SubRoleId = SubRoleId,
                        PasswordHash = hashedPassword,
                        PasswordSalt = Salt,
                        ConfirmationCode = ConfirmationCode,
                        SessionId = subscription.SessionId,
                        SubscriptionId = subscription.SubscriptionId,
                        TierId = subscription.TierId
                    };
                    var login = unitOfWork.GetRepository<User>();
                    var newuser = login.Insert(mappedRequest);
                    login.Save();
                    //int BusinessId = SaveUserAddress(dto, newuser.Id);
                    response.Status = "Your account verification is pending. Please check your email and click the verification link to activate your account.\r\n\r\nIf you don’t see the email, check your spam folder or request a new verification email.";
                    response.Name = dto.Name;
                    response.Id = newuser.Id;
                    response.Email = dto.Email;
                    response.RoleId = RoleID;

                    EmailContent emailContent = new()
                    {
                        BaseUrl = _config["JwtSettings:Issuer"],
                        Name = dto.Name,
                        Email = dto.Email,
                        ClickLink = "/ActivateAccount?code=" + ConfirmationCode,
                        CopyRightYear = DateTime.Now.Year.ToString(),
                        LogoLink = "/images/logo.png",
                        OtherText = password
                    };
                    var ContentToFill = _emailHelper.FillEmailContents(emailContent, "verify_email", dto.Name);
                    _emailService.SendEmail(dto.Email, "Account Verification Required", ContentToFill, true);
                }
            }
            catch
            {
                response.Status = "An unexpected error occurred";
                throw;
            }
            return response;
        }

        private int SaveUserAddress(BusinessCreditDto dto, int UserId)
        {
            var repository = unitOfWork.GetRepository<UserAddress>();

            UserAddress businesses = new()
            {
                UserId = UserId,
                AddedById = UserId,
                ModifiedById = UserId,
                AddedOn = DateTime.Now,
                Address = dto.Address,
                City = dto.City,
                ContactPerson = dto.ContactPerson,
                Description = dto.Description,
                Id = dto.Id,
                IsDeleted = false,
                ModifiedOn = DateTime.Now,
                Phone = dto.Phone,
                State = dto.State,
                ZipCode = dto.ZipCode
            };
            var response = repository.Insert(businesses);
            repository.Save();
            return response.Id;
        }

        public AddressDto GetMyProfile()
        {
            var userid = _userService.GetUserId();
            var repository = unitOfWork.GetRepository<User>();
            var entity = repository.GetByInclude(u => u.Id == userid, u => u.Businesses, u => u.Roles, u => u.SubRoles);
            AddressDto business = new();

            business = _mapper.Map<AddressDto>(entity);
            if (entity.RoleId == (int)RoleEnum.Users || entity.RoleId== (int)RoleEnum.Business)
            {
                var subscription = _subscription.Value.GetSubscriptionById(entity.SubscriptionId);
                if (subscription != null)
                {
                    business.subscriptionDetail = subscription;
                    business.subscriptionDetail.SessionId = entity.SessionId;
                }
            }
            return business;
        }
        public bool IsUserExists(string email, int Id = 0)
        {
            var repository = unitOfWork.GetRepository<User>();
            return repository != null && repository.GetAll().Any(x => (Id == 0 || x.Id != Id) && x.Email == email);
        }
        public ResponseDto UpdateBusiness(AddressDto business)
        {
            var response = new ResponseDto();

            try
            {
                if (IsUserExists(business.Email, business.UserId))
                {
                    response.Id = -1;
                    response.Status = "Email already exists";
                }
                else
                {
                    var repository = unitOfWork.GetRepository<User>();

                    var entity = repository.GetById(business.UserId);
                    entity.Email = business.Email;
                    entity.FullName = business.Name;
                    entity.ModifiedById = business.UserId;
                    entity.ModifiedOn = DateTime.Now;
                    repository.Update(entity);
                    repository.Save();

                    var bizrepository = unitOfWork.GetRepository<UserAddress>();
                    var address = bizrepository.GetById(business.Id);

                    if (address != null)
                    {
                        var mappedaddress = _mapper.Map(business, address);

                        bizrepository.Update(mappedaddress);
                        bizrepository.Save();
                    }
                    else
                    {
                        var mappedaddress = _mapper.Map<BusinessCreditDto>(business);
                        SaveUserAddress(mappedaddress, business.UserId);
                    }

                    response.Id = 1;
                    response.Status = "Profile information updated successfully";
                }
            }
            catch
            {
                response.Status = "An unexpected error occurred";
                throw;
            }
            return response;
        }
        public bool ActivateAccount(string code)
        {
            var repository = unitOfWork.GetRepository<User>();
            var response = repository.FirstOrDefault(x => x.ConfirmationCode == code);

            if (response != null)
            {
                response.ConfirmationCode = string.Empty;
                response.ConfirmedOn = response.ModifiedOn = DateTime.Now;
                response.AddedById = response.ModifiedById = response.Id;
                response.IsConfirmed = true;
                repository.Update(response);
                repository.Save();

                EmailContent emailContent = new()
                {
                    BaseUrl = _config["JwtSettings:Issuer"],
                    Name = response.FullName,
                    Email = response.Email,
                    ClickLink = "/Login",
                    CopyRightYear = DateTime.Now.Year.ToString(),
                    LogoLink = "/images/logo.png",
                    // OtherText = "Password : "+ 
                };
                var ContentToFill = _emailHelper.FillEmailContents(emailContent, "accountactivation", response.FullName);
                _emailService.SendEmail(response.Email, "Account is successfully created!", ContentToFill, true );

                return true;
            }
            return false;
        }
        public void UpdateSubscription(string SessionId,int UserId)
        { 
            var repository = unitOfWork.GetRepository<User>();
            var entity = repository.GetById(UserId);
            var subscription = _subscription.Value.GetSubscriptionBySessionId(SessionId);
            entity.SubscriptionId = subscription.Id;
            entity.SessionId = SessionId;
            entity.ModifiedById = UserId;
            entity.ModifiedOn = DateTime.Now;
            repository.Update(entity);
            repository.Save(); 
        }
    }
}
