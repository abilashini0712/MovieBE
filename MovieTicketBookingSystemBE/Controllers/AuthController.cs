
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.LoginDto;
using MovieTicketBookingSystemBE.Models;
using MovieTicketBookingSystemBE.Services;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;

        private readonly PasswordHasher<Register>
            _passwordHasher = new PasswordHasher<Register>();

        public AuthController(
            ApplicationDbContext context,
            IConfiguration configuration,
            JwtService jwtService)
        {
            _context = context;
            _configuration = configuration;
            _jwtService = jwtService;
        }

       
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] Register model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(
                    "Name, email, and password are required.");
            }

            var email = model.Email.Trim();

            var existingUser = await _context.Registers
                .FirstOrDefaultAsync(
                    x => x.Email == email);

            if (existingUser != null)
            {
                return BadRequest(
                    "Email already registered.");
            }

            model.Name = model.Name.Trim();
            model.Email = email;

            model.Password = _passwordHasher.HashPassword(
                model,
                model.Password);

            _context.Registers.Add(model);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful"
            });
        }

        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(
                    "Username/email and password are required.");
            }

            var userName = model.UserName.Trim();

            
            var adminEmail =
                _configuration["AdminSettings:Email"];

            var adminPassword =
                _configuration["AdminSettings:Password"];

            if (!string.IsNullOrWhiteSpace(adminEmail) &&
                !string.IsNullOrEmpty(adminPassword) &&
                string.Equals(
                    userName,
                    adminEmail,
                    StringComparison.OrdinalIgnoreCase) &&
                model.Password == adminPassword)
            {
                var adminToken = _jwtService.GenerateToken(
                    adminEmail,
                    "Admin",
                    "0");

                return Ok(new
                {
                    message = "Login successful",
                    token = adminToken,
                    userId = 0,
                    name = "Admin",
                    email = adminEmail,
                    isAdmin = true
                });
            }

           
            var matchingUsers = await _context.Registers
                .Where(x =>
                    x.Email == userName ||
                    x.Name == userName)
                .ToListAsync();

            var user = matchingUsers.FirstOrDefault(x =>
                _passwordHasher.VerifyHashedPassword(
                    x,
                    x.Password,
                    model.Password)
                != PasswordVerificationResult.Failed);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid username/email or password"
                });
            }

            var token = _jwtService.GenerateToken(
                user.Email,
                "User",
                user.Id.ToString());

            return Ok(new
            {
                message = "Login successful",
                token = token,
                userId = user.Id,
                name = user.Name,
                email = user.Email,
                isAdmin = false
            });
        }
    }
}
