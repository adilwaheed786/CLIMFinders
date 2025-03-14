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
    public class SearchController(IEmailService emailService, ISearchService searchService, ILogger<SearchController> logger) : ControllerBase
    {
        private readonly ISearchService _searchService = searchService;
        private readonly ILogger<SearchController> _logger = logger;
        private readonly IEmailService _emailService = emailService;
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
        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFileAndSendEmail([FromForm] IFormFile file, [FromForm] string emailAddress)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File is required." });
            }

            // File save path
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            string filePath = Path.Combine(uploadsFolder, file.FileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Send Email with Attachment
            string subject = "Uploaded Vehicle File";
            string message = "Please find the attached file.";

            await _emailService.SendEmailWithAttachment(subject, message, filePath);

            // Delete file after sending
            System.IO.File.Delete(filePath);

            return Ok(new { message = "File uploaded and email sent successfully." });
        }
    }
}

