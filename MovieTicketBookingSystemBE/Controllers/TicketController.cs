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
        public async Task<ActionResult<TicketDto>> GetTicket(int BookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var ticket = new TicketDto
            {
                Date = booking.Date,
                Time = booking.Time,
                Cinemas = booking.Cinemas,
                Seats = booking.Seats,
                BookingId = booking.Id
            };

            return Ok(ticket);
        }
    }
}
