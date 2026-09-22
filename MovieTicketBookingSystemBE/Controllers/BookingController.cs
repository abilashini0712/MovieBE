using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using NuGet.Protocol.Core.Types;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        


        [HttpPost]
        public async Task<IActionResult> Booking(Booking booking)
        {



            if (booking.Tickets <= 0)
            {
                return BadRequest("Number of tickets must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(booking.Seats))
            {
                return BadRequest("Please select at least one seat.");
            }

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            return Ok(booking);
        }


      /* [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Register)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Register)
                .FirstOrDefaultAsync(b => b.id == id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }*/
      
    }
}
