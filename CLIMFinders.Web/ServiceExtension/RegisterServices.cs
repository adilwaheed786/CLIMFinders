using CLIMFinders.Application.Interfaces;
using CLIMFinders.Application.Services;
using CLIMFinders.Infrastructure.Repositories;
using CLIMFinders.Repositories;
using CLIMFinders.StripeProcess;
using CLIMFinders.StripeProcess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace CLIMFinders.Web.ServiceExtension
{
    public static class RegisterServices
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWorkBase>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHashManager, HashManager>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStaticSelectOptionService, StaticSelectOptionService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IEmailHelperUtils, EmailHelperUtils>();
            services.AddScoped<ISubscriptionPlanServices, SubscriptionPlanServices>();
            services.AddScoped<Lazy<ISubscriptionPlanServices>>(sp =>
             new Lazy<ISubscriptionPlanServices>(() => sp.GetRequiredService<ISubscriptionPlanServices>()));
            services.AddHttpContextAccessor();
        }
    }
}
