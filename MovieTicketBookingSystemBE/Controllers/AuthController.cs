using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            var existingUser = await _context.Registers
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already registered.");
            }

            _context.Registers.Add(model);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            var user = await _context.Registers
                .FirstOrDefaultAsync(x =>
                    (x.Email == model.UserName || x.Name == model.UserName)
                    && x.Password == model.Password);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username/email or password"
                });
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.Id,
                name = user.Name,
                email = user.Email
            });
        }
    }


    }
