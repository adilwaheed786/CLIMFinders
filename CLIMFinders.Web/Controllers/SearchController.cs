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
        public async Task<IActionResult> UploadFileAndSendEmail([FromForm] DocumentUploadDto model)
        {
            if (model.Attachment == null || model.Attachment.Length == 0)
            {
                return BadRequest(new { message = "File is required." });
            }

            // File save path
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            string filePath = Path.Combine(uploadsFolder, model.Attachment.FileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Attachment.CopyToAsync(stream);
            }

            // Prepare email content
            string subject = "Vehicle Document Submission";
            string message = $@"
        <p><strong>Name:</strong> {model.Name}</p>
        <p><strong>Email:</strong> {model.Email}</p>
        <p><strong>Details:</strong> {model.Details}</p>
        <p><strong>VIN:</strong> {model.VIN}</p>
        <p>Please find the attached document.</p>";

            // Send email with attachment
            await _emailService.SendEmailWithAttachment(subject, message, filePath);

            // Delete file after sending
            System.IO.File.Delete(filePath);

            return Ok(new { message = "File uploaded and email sent successfully." });
        }
    }
}

