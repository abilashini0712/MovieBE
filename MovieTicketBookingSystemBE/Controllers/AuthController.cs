using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTOs;
using MovieTicketBookingSystemBE.Models;
using System;

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
        public async Task<IActionResult> Register(RegisterDto dto)
        {
           
            var existingUser = await _context.Registers
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

         
            var user = new Register
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Registers.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration successful",
                registerId = user.RegisterId
            });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Registers
                .FirstOrDefaultAsync(x =>
                    x.Email == dto.Email &&
                    x.Password == dto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                message = "Login successful",
                id = user.RegisterId,
                name = user.Name,
                email = user.Email
            });
        }
    }
}