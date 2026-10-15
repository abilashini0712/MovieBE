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
    //[Authorize]
    [ApiController] //automatically validate the models
    [Route("api/[controller]")] //define the base url for the controller
    public class AuthController : ControllerBase // (ControllerBase) provide fuctionality needed for the web Api
    {
        private readonly ApplicationDbContext _context;
       
        
        private readonly IConfiguration _configuration;

        private readonly JwtService _jwtService;
       
        private readonly PasswordHasher<Register> _passwordHasher = new PasswordHasher<Register>();

        public AuthController(ApplicationDbContext context, IConfiguration configuration, JwtService jwtService )
        {
            _context = context;
            _configuration = configuration;
            _jwtService = jwtService;
            

        }

        //[AllowAnonymous]
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        // in here public method  access by the API framework , async nmethod aynchronus databae operation, & (Task<IActionResult>) method will eventually return an http response.
        {
            var existingUser = await _context.Registers
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already registered.");
            }

            
            model.Password = _passwordHasher.HashPassword(model, model.Password);

           
            _context.Registers.Add(model);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful"
            });
        }

        //[AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            
            var adminEmail = _configuration["AdminSettings:Email"];
            var adminPassword = _configuration["AdminSettings:Password"];

            if (!string.IsNullOrEmpty(adminEmail)
                && !string.IsNullOrEmpty(adminPassword)
                && model.UserName == adminEmail
                && model.Password == adminPassword)
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
                .Where(x => x.Email == model.UserName || x.Name == model.UserName)
                .ToListAsync();

             
            var user = matchingUsers.FirstOrDefault(x =>
                _passwordHasher.VerifyHashedPassword(x, x.Password, model.Password)
                != PasswordVerificationResult.Failed);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username/email or password"
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