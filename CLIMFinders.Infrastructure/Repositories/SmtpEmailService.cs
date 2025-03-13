using CLIMFinders.Application.Interfaces;
using Microsoft.Extensions.Options; 
using CLIMFinders.Application.DTOs;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class SmtpEmailService(IOptions<SmtpSettings> smtpSettings) : IEmailService
    {
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="Isadmin">True if copy all emails back to Sender's Email</param>
        public void SendEmail(
            string emailAddress,
            string subject,
            string message, bool Isadmin = false
        )
        {
            Execute(subject, message, emailAddress, Isadmin);            
        }
        private void Execute(
           string subject,
           string message,
           string emailAddress, bool Isadmin
       )
        {
            var smtpProvider = _smtpSettings.Server;
            var portNumber = Convert.ToInt32(_smtpSettings.Port);
            var user = _smtpSettings.Username;
            var password = _smtpSettings.Password;
            var sender = _smtpSettings.NoreplyFrom;
            string EmailAddress = Isadmin ? _smtpSettings.NoreplyFrom : emailAddress;
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", sender));
            if (!Isadmin)
            {
                emailMessage.Bcc.Add(new MailboxAddress("", _smtpSettings.AdminEmail));
            }
            emailMessage.To.Add(new MailboxAddress("", EmailAddress));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpProvider, portNumber, SecureSocketOptions.Auto);
                client.Authenticate(user, password);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        } 
    }
}
