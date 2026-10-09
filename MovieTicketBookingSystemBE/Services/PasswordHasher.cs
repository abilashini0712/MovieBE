
using System.Security.Cryptography;
using System.Text;

namespace MovieTicketBookingSystemBE.Services
{
    public class PasswordHasher
    {
        private const int SALT_SIZE = 16;
        private const int ITERATIONS = 350000;
        private const int HASH_SIZE = 64;

        public string GenerateHash(string password, string salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Convert.FromBase64String(salt),
                ITERATIONS,
                HashAlgorithmName.SHA512,
                HASH_SIZE
            );

            return Convert.ToBase64String(hash);
        }

        public string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
            return Convert.ToBase64String(salt);
        }
    }
}
