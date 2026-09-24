using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TicketController :ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int Id)
        {
            var ticket = await _context.Tickets.FindAsync(Id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }
    }
}
