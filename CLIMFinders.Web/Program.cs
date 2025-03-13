using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Enums;
using CLIMFinders.Domain.Entities;
using CLIMFinders.Infrastructure.Data;
using CLIMFinders.Web.ServiceExtension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Stripe;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();
logger.Info("Application is starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;

    // Configure JWT Authentication Settings
    var jwtSettings = config.GetSection("JwtSettings");
    var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

    // Enable CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.WithOrigins(jwtSettings["Issuer"])
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });

    // Configure Logging with NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Register Services
    builder.Services.ConfigureRepositoryWrapper();

    builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
    builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
    );
    // Add Controllers and Razor Pages
    builder.Services.AddControllers();
    builder.Services.AddRazorPages();

    // Configure Database
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Configure JWT Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKey)
            };
        });

    // Configure Authorization Policies
    builder.Services.AddAuthorizationBuilder()
            .AddPolicy("SuperAdminPolicy", policy => policy.RequireRole(RoleEnum.SuperAdmin.ToString()))
            .AddPolicy("UsersPolicy", policy => policy.RequireRole(RoleEnum.Users.ToString()))
            .AddPolicy("BusinessPolicy", policy => policy.RequireRole(RoleEnum.Business.ToString()))
            .AddPolicy("ActiveSubscription", policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var subscriptionClaim = context.User.FindFirst(CustomClaimTypes.ActiveSubscription);
                    return subscriptionClaim != null && subscriptionClaim.Value == "true";
                });
            });


    // Register AutoMapper
    builder.Services.AddAutoMapper(typeof(GenericMappingProfile));

    // Add Stripe configuration to DI container
    builder.Services.AddSingleton<IStripeClient>(sp =>
    {
        var stripeSecretKey = builder.Configuration["Stripe:SecretKey"];
        return new StripeClient(stripeSecretKey);
    });

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

    var app = builder.Build();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseStaticFiles();
    // Configure Middleware Pipeline
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    app.UseMiddleware<JwtCookieMiddleware>(); // Custom Middleware to Extract JWT from Cookies
   
    app.UseRouting();
    app.UseAuthentication();

    app.UseAuthorization();
    // Middleware to handle Unauthorized & Subscription Redirects
    app.Use(async (context, next) =>
    {
        await next();

        if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            var user = context.User;
            var subscriptionClaim = user.FindFirst(CustomClaimTypes.ActiveSubscription);
            if(context.Request.Path.StartsWithSegments("/api/"))
            {
                context.Response.ContentType = "application/json";
            }
            else if (subscriptionClaim == null || subscriptionClaim.Value != "True")
            {
                context.Response.Redirect("/SubscriptionRenew");
            }
            else
            {
                context.Response.Redirect("/Unauthorized");
            }
        }
        else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            context.Response.Redirect("/Login");
        }
    });
   // app.UseMiddleware<AuthorizationRedirectMiddleware>();
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
    });

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application startup failed.");
    throw;
}
finally
{
    LogManager.Shutdown(); // Ensure proper shutdown of NLog
}
