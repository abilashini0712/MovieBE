
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.UserDto;
using MovieTicketBookingSystemBE.Models;
using MovieTicketBookingSystemBE.Services;
using System.Security.Cryptography;
using System.Text;

namespace MovieTicketBookingSystemBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) ||
                string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var existingUser = await context.users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }

            PasswordHasher hasher = new PasswordHasher();

            string salt = hasher.GenerateSalt();
            string passwordHash = hasher.GenerateHash(
                userDto.Password, salt);

            users newUser = new users
            {
                Email = userDto.Email,
                PasswordSalt = salt,
                PasswordHash = passwordHash
            };

            context.users.Add(newUser);
            await context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) ||
                string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await context.users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null)
            {
                return BadRequest(
                    "You are not registered. Please register first.");
            }

            if (string.IsNullOrWhiteSpace(user.PasswordSalt) ||
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return BadRequest("Stored password data is invalid.");
            }

            var passwordHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(userDto.Password),
                Convert.FromBase64String(user.PasswordSalt),
                350000,
                HashAlgorithmName.SHA512,
                64
            );

            byte[] storedHash;

            try
            {
                storedHash = Convert.FromBase64String(user.PasswordHash);
            }
            catch (FormatException)
            {
                return BadRequest("Stored password data is invalid.");
            }

            bool compareResult =
                passwordHash.Length == storedHash.Length &&
                CryptographicOperations.FixedTimeEquals(
                    passwordHash, storedHash);

            if (compareResult)
            {
                return Ok("Login successful.");
            }

            return Unauthorized("Invalid email or password.");
        }
    }
}
