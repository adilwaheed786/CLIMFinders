using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace CLIMFinders.Web.Pages
{
    public class TestModel(ILogger<TestModel> logger, IEmailService emailService) : PageModel
    {
        private readonly IEmailService _emailService = emailService;

        private readonly ILogger<TestModel> _logger = logger;

        public void OnGet()
        {
            //string[] hash = new string[2];
            //string password = "Password@!@$1";
            //byte[] saltBytes = new byte[16];
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(saltBytes);
            //}
            //// Salt
            //hash[0] = Convert.ToBase64String(saltBytes);


            //using (var sha256 = SHA256.Create())
            //{
            //    byte[] saltedPassword = Encoding.UTF8.GetBytes(password + hash[0]);
            //    byte[] hashBytes = sha256.ComputeHash(saltedPassword);
            //    hash[1] = Convert.ToBase64String(hashBytes);
            //}
            //Console.WriteLine(hash[0]);
            //Console.WriteLine(hash[1]);

        }
    }
}
