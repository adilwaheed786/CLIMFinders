using Azure;
using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CLIMFinders.Web.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController(ISearchService searchService, ILogger<SearchController> logger) : ControllerBase
    {
        private readonly ISearchService _searchService = searchService;
        private readonly ILogger<SearchController> _logger = logger;

        [HttpGet("searchbyvin")]
        public IActionResult SearchByVin(string vin) 
        {
            SearchResultDto resultDto = new();
            var subscriptionClaim = User.FindFirst(CustomClaimTypes.ActiveSubscription);
            if (subscriptionClaim == null || subscriptionClaim.Value != "True")
            {
                resultDto.Status = "403";
            }
            else
            {
                var response = _searchService.GetSearchResult(vin);
                resultDto.Result = response; 
                // Check Subscription Status
            }
            return Ok(new { data = resultDto });
        }
    }
}
