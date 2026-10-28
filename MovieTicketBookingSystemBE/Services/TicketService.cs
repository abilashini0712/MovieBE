
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task IssueTicketsAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                throw new InvalidOperationException(
                    "Booking was not found.");
            }

            // Do not create tickets twice for the same booking.
            bool ticketsExist = await _context.Tickets
                .AnyAsync(t => t.BookingId == bookingId);

            if (ticketsExist)
            {
                return;
            }

            // Example: "A1, A2" becomes two separate seats.
            var seats = booking.Seats
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (seats.Count == 0)
            {
                throw new InvalidOperationException(
                    "No seats were found for this booking.");
            }

            var tickets = new List<Ticket>();

            foreach (var seat in seats)
            {
                tickets.Add(new Ticket
                {
                    BookingId = booking.Id,
                    Date = booking.Date,
                    Time = booking.Time,
                    Cinemas = booking.Cinemas,
                    Seats = seat,

                    QrToken = Convert.ToHexString(
                        RandomNumberGenerator.GetBytes(32)),

                    IsUsed = false
                });
            }

            _context.Tickets.AddRange(tickets);

            await _context.SaveChangesAsync();
        }
    }
}
