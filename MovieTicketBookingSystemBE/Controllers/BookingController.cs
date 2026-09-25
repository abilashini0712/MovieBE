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




        /*   [HttpPost]
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

               var b = new Booking{
                   Seats = booking.Seats,
                   Time = booking.Time,
                   Cinemas = booking.Cinemas,
                   Date = booking.Date,
                   Tickets = booking.Tickets,
                   RegisterId = booking.RegisterId
               };
               _context.Bookings.Add(b);

               await _context.SaveChangesAsync();

               return Ok(b);
           } */

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

            await _context.SaveChangesAsync();

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
       
    }
}
