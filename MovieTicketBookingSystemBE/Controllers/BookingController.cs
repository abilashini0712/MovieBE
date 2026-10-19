using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.BookingDto;
using MovieTicketBookingSystemBE.Models;
using NuGet.Protocol.Core.Types;

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

        public async Task<IActionResult> Booking(CreateBookingDto booking)
        {
            if (booking.Tickets <= 0)
            {
                return BadRequest("Number of tickets must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(booking.Seats))
            {
                return BadRequest("Please select at least one seat.");
            }


            var registerExists = await _context.Registers
                .AnyAsync(r => r.Id == booking.RegisterId);

            Console.WriteLine($"RegisterId received: {booking.RegisterId}");
            Console.WriteLine($"Register exists: {registerExists}");
            Console.WriteLine($"Date received by API: {booking.Date:yyyy-MM-dd}"); //this code newly added for testing the booking date

            if (!registerExists)
            {
                return BadRequest(
                    $"RegisterId {booking.RegisterId} does not exist in Registers table.");
            }

            var b = new Booking
            {
                Seats = booking.Seats,
                Time = booking.Time,
                Cinemas = booking.Cinemas,
                Date = booking.Date,
                Tickets = booking.Tickets,
                RegisterId = booking.RegisterId
            };

            _context.Bookings.Add(b);

            Console.WriteLine($"Date before save: {b.Date:yyyy-MM-dd}");


            await _context.SaveChangesAsync();

            var savedBooking = await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == b.Id);

            Console.WriteLine($"Date read from database: {savedBooking?.Date:yyyy-MM-dd}");

            return Ok(b);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetToBook(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();

            return Ok(bookings);
        }

    }
}