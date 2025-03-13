using CLIMFinders.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class HashManager : IHashManager
    {
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            string enteredHash = HashPassword(enteredPassword, storedSalt);
            return storedHash == enteredHash;
        } 
    }
}
