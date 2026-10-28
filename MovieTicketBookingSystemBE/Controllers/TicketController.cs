//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MovieTicketBookingSystemBE.Data;
//using MovieTicketBookingSystemBE.DTO.TicketDto;
//using MovieTicketBookingSystemBE.Models;

//namespace MovieTicketBookingSystemBE.Controllers
//{
//    //[Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TicketController :ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public TicketController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        //[AllowAnonymous]
//        [HttpGet("{BookingId}")]
//        public async Task<ActionResult<TicketDto>> GetTicket(int BookingId)
//        {
//            var booking = await _context.Bookings
//                .FirstOrDefaultAsync(b => b.Id == BookingId);

//            if (booking == null)
//            {
//                return NotFound();
//            }

//            var ticket = new TicketDto
//            {
//                MovieName = booking.MovieName,
//                Date = booking.Date,
//                Time = booking.Time,
//                Cinemas = booking.Cinemas,
//                Seats = booking.Seats,
//                BookingId = booking.Id
//            };

//            return Ok(ticket);
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.TicketDto;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{BookingId:int}")]
        public async Task<ActionResult<List<TicketDto>>> GetTicket(
            int BookingId)
        {
            var tickets = await _context.Tickets
                .AsNoTracking()
                .Include(t => t.Booking)
                .Where(t => t.BookingId == BookingId)
                .OrderBy(t => t.Id)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    BookingId = t.BookingId,
                    MovieName = t.Booking!.MovieName,
                    Date = t.Date,
                    Time = t.Time,
                    Cinemas = t.Cinemas,
                    Seats = t.Seats,
                    QrToken = t.QrToken,
                    IsUsed = t.IsUsed
                })
                .ToListAsync();

            if (tickets.Count == 0)
            {
                return NotFound(
                    "No tickets found. Payment may not be confirmed yet.");
            }

            return Ok(tickets);
        }
    }
}
