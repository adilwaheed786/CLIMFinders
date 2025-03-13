using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.Interfaces
{
    public interface IHashManager
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}
