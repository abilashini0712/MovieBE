using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;
using MovieTicketBookingSystemBE.Services;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(
            ApplicationDbContext context,
            IEmailService emailService,
            ILogger<ContactController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            contact.Id = 0;
            contact.CreatedAt = DateTime.UtcNow;
            contact.Status = "New";

            
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

          
            try
            {
                await _emailService.SendContactEmailAsync(contact);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Failed to send contact email for message {Id}", contact.Id);

                contact.Status = "EmailFailed";
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Contact message saved successfully." });
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