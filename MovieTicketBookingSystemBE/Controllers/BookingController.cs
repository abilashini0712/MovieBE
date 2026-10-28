using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using NuGet.Protocol.Core.Types;
using MovieTicketBookingSystemBE.Models;
using MovieTicketBookingSystemBE.DTO.BookingDto;

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

            if (!registerExists)
            {
                return BadRequest(
                    $"RegisterId {booking.RegisterId} does not exist in Registers table.");
            }

          

                  Console.WriteLine(
            $"API received date: {booking.Date:yyyy-MM-dd}");

            var b = new Booking
            {
                Seats = booking.Seats,
                Time = booking.Time,
                Cinemas = booking.Cinemas,
                Date = booking.Date.Date,
                Tickets = booking.Tickets,
                RegisterId = booking.RegisterId,
                MovieName = booking.MovieName
            };

           
            Console.WriteLine($"Date received by API: {booking.Date:yyyy-MM-dd}");
            Console.WriteLine($"Date assigned to entity: {b.Date:yyyy-MM-dd}");

            _context.Bookings.Add(b);

            await _context.SaveChangesAsync();

            
            Console.WriteLine($"Booking ID: {b.Id}");
            Console.WriteLine($"Date after saving: {b.Date:yyyy-MM-dd}");

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



        [HttpGet("available-seats")]
        public async Task<IActionResult> GetAvailableSeats(
            [FromQuery] DateTime date,
            [FromQuery] string time,
            [FromQuery] string cinema,
            [FromQuery] string movieName)
        {
            if (string.IsNullOrWhiteSpace(time) ||
                string.IsNullOrWhiteSpace(cinema) ||
                string.IsNullOrWhiteSpace(movieName))
            {
                return BadRequest(
                    "Date, time, cinema, and movie name are required.");
            }

            var bookings = await _context.Bookings
                .Where(b =>
                    b.Date.Date == date.Date &&
                    b.Time == time &&
                    b.Cinemas == cinema &&
                    b.MovieName == movieName)
                .Select(b => b.Seats)
                .ToListAsync();

            var reservedSeats = bookings
                .Where(seats => !string.IsNullOrWhiteSpace(seats))
                .SelectMany(seats => seats.Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries))
                .Select(seat => seat.Trim())
                .Distinct()
                .ToList();

            return Ok(reservedSeats);
        }


    }
}