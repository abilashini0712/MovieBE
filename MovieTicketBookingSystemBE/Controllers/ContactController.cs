using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] Contact contact)
        {
            contact.Id = 0;
            contact.CreatedAt = DateTime.UtcNow;
            contact.Status = "New";

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Contact message saved successfully."
            });
        }

    
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _context.Contacts
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(messages);
        }
    }
}