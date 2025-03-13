using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.Interfaces
{
    public interface IEmailService
    {
        // void SendEmail(string emailAddress, string subject, string body, string fromEmail = null, string displayName = null);
        void SendEmail(string emailAddress, string subject, string message, bool Isadmin = false); 
    }
}
 