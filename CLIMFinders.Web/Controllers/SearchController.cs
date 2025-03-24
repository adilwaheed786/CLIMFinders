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
            if (model.Attachments == null || model.Attachments.Count < 4)
            {
                return BadRequest(new { message = "All required documents must be uploaded." });
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            List<string> savedFilePaths = new List<string>();
            List<string> fileDetails = new List<string>();

            // Map file types to their respective names
            string[] fileTypeNames = { "Proof of Ownership", "Valid Photo ID", "Proof of Insurance", "Payment Receipt" };

            int index = 0;
            foreach (var file in model.Attachments)
            {
                if (file.Length > 0)
                {
                    // Create unique file name
                    string uniqueFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}_{file.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    savedFilePaths.Add(filePath);

                    // Add file details for email
                    if (index < fileTypeNames.Length)
                    {
                        fileDetails.Add($"<p><strong>{fileTypeNames[index]}:</strong> {file.FileName}</p>");
                    }

                    index++;
                }
            }

            // Prepare email content
            string subject = "Vehicle Document Submission";
            string message = $@"
        <p><strong>Name:</strong> {model.Name}</p>
        <p><strong>Email:</strong> {model.Email}</p>
        <p><strong>VIN:</strong> {model.VIN}</p>
        <p>Please find the attached required documents:</p>
        {string.Join("\n", fileDetails)}
    ";

            // Send email with attachments
            await _emailService.SendEmailWithAttachments(subject, message, savedFilePaths);

            // Delete files after sending email
            foreach (var filePath in savedFilePaths)
            {
                System.IO.File.Delete(filePath);
            }

            return Ok(new { message = "All documents uploaded and email sent successfully." });
        }

    }
}

