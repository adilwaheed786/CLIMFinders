using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIMFinders.Web.Pages
{ 
    public class IndexModel(ISubscribeService service) : PageModel
    {
        private readonly ISubscribeService _service = service;

        public void OnGet()
        {
            InputPlans = _service.GetSubscriptionPlans();
        } 
        public List<SubscriptionPlansDto> InputPlans { get; set; } = [];
    }
}
