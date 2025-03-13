using Azure.Core;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Forwarding;

namespace CLIMFinders.Web.Pages
{ 
    public class SubscriptionCancelModel() : PageModel
    { 
        public void OnGet()
        {
        }
    }
}
