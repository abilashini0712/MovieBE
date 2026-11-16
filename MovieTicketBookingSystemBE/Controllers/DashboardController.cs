using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.DashboardDto;

using Microsoft.EntityFrameworkCore;

namespace MovieTicketBookingSystemBE.Controllers
{

     [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var result = new DashboardDto();

            // Total movies
            result.TotalMovies = await _context.Movies.CountAsync();

            // Total bookings
            result.TotalBookings = await _context.Bookings.CountAsync();

            // Total registered users
            result.TotalUsers = await _context.Registers.CountAsync();

            // TODO: Calculate revenue using successful payments.


            // Get successful payments
            var successfulPayments = _context.Payments
                .Where(p => p.Status == "Paid");

            var totalAmount = await successfulPayments
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            // Convert cents into dollars
            result.TotalRevenue = totalAmount / 100m;



            var currentMonth = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1);

            var startMonth = currentMonth.AddMonths(-11);

            var monthlyPayments = await _context.Payments
                .Where(p =>
                    p.Status == "Paid" &&
                    p.CreatedAt >= startMonth &&
                    p.CreatedAt < currentMonth.AddMonths(1))
                .Select(p => new
                {
                    p.CreatedAt,
                    p.Amount
                })
                .ToListAsync();

            // Create 12 months of data
            for (int i = 0; i < 12; i++)
            {
                var month = startMonth.AddMonths(i);

                var income = monthlyPayments
                    .Where(p =>
                        p.CreatedAt.Year == month.Year &&
                        p.CreatedAt.Month == month.Month)
                    .Sum(p => (double)p.Amount) / 100.0;

                result.MonthlyIncome.Add(income);
            }


            // Use your actual Payment amount and status properties.

            // Recent bookings
            result.RecentBookings = await _context.Bookings
                .OrderByDescending(b => b.Date)
                .Take(5)
                .Select(b => new RecentBookingDto
                {
                    BookingId = b.Id,
                    User = b.RegisterId.ToString(),
                    MovieName = b.MovieName,
                    Seats = b.Seats,
                    Date = b.Date
                })
                .ToListAsync();

            // Top performing movies
            result.TopMovies = await _context.Bookings
                .GroupBy(b => b.MovieName)
                .Select(g => new TopMovieDto
                {
                    MovieName = g.Key,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToListAsync();

            return Ok(result);
        }
    }

}
