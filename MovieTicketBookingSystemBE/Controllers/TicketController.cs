using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.TicketDto;
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

        [HttpGet("{BookingId}")]
        public async Task<ActionResult<Ticket>> GetTicket(int BookingId)
        {
            var ticket = await _context.Tickets.FindAsync(BookingId);


         
            if (ticket == null)
            {
                return NotFound();
            }

            var tick = new TicketDto
            {
                Date = ticket.Date,
                Time = ticket.Time,
                Cinemas = ticket.Cinemas,
                Seats = ticket.Seats,
                BookingId =ticket.BookingId
            };

            return ticket;
        }
    }
}
