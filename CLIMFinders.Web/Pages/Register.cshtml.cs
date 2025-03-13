using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CLIMFinders.Web.Pages
{ 
    public class RegisterModel(ISubscribeService service) : PageModel
    {
        private readonly ISubscribeService _service = service;

        public void OnGet()
        {
            InputPlans = _service.GetSubscriptionPlans();
        }
        public List<SubscriptionPlansDto> InputPlans { get; set; } = [];
    }
}
