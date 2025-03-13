using CLIMFinders.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CLIMFinders.Web.ServiceExtension
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeAttribute(params string[] roles) : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles = roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // If user is not authenticated, redirect to Login
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.HttpContext.Response.Redirect("/Login");
                return;
            }

            // If roles are specified, enforce role-based access control
            if (_roles.Length > 0 && !_roles.Any(role => user.IsInRole(role)))
            {
                context.HttpContext.Response.Redirect("/Unauthorized");
                return;
            }

            // Check for Active Subscription Claim (only if required)
            var subscriptionClaim = user.FindFirst(CustomClaimTypes.ActiveSubscription);
            if (subscriptionClaim == null || subscriptionClaim.Value != "True")
            {
                context.HttpContext.Response.Redirect("/SubscriptionRenew");
                return;
            }

            // If no roles are specified, allow all authenticated users
            // This means every authenticated user can access unless restricted by subscription status.
        }
    }
}
