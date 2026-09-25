using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; // this give API conntrollers
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;
using MovieTicketBookingSystemBE.DTO.LoginDto;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController] //automatically validate the models
    [Route("api/[controller]")] //define the base url for the controller
    public class AuthController : ControllerBase // (ControllerBase) provide fuctionality needed for the web Api
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context) 
        {
            _context = context;
        }

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

            _context.Registers.Add(model);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
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
