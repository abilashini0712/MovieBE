
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.PaymentDto;
using MovieTicketBookingSystemBE.Models;
using Stripe;
using Stripe.Checkout;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession(
            CreateStripeCheckoutDto dto)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == dto.BookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            if (booking.Tickets <= 0)
                return BadRequest("Invalid ticket quantity.");

            var priceText = _configuration[
                "TicketPricing:PricePerTicketMinorUnits"];

            if (!long.TryParse(priceText, out long pricePerTicket)
                || pricePerTicket <= 0)
            {
                return StatusCode(500,
                    "Ticket pricing is not configured.");
            }

            var currency = _configuration[
                "TicketPricing:Currency"] ?? "usd";

            long totalAmount;

            try
            {
                totalAmount = checked(
                    pricePerTicket * booking.Tickets);
            }
            catch (OverflowException)
            {
                return BadRequest("Invalid payment amount.");
            }

            var secretKey = _configuration["Stripe:SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
                return StatusCode(500,
                    "Stripe is not configured.");

            StripeConfiguration.ApiKey = secretKey;

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = totalAmount,
                Currency = currency,
                Status = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var options = new SessionCreateOptions
            {
                Mode = "payment",

                SuccessUrl =
                    "https://localhost:7122/paymentResult"
                    + "?session_id={CHECKOUT_SESSION_ID}",

                CancelUrl =
                    $"https://localhost:7122/payment/{booking.Id}",

                ClientReferenceId = booking.Id.ToString(),

                Metadata = new Dictionary<string, string>
                {
                    ["PaymentId"] = payment.Id.ToString(),
                    ["BookingId"] = booking.Id.ToString()
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = booking.Tickets,

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,

                            UnitAmount = pricePerTicket,

                            ProductData =
                                new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Movie Ticket"
                                }
                        }
                    }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            payment.StripeSessionId = session.Id;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                checkoutUrl = session.Url
            });
        }

        [HttpGet("status/{sessionId}")]
        public async Task<IActionResult> GetPaymentStatus(string sessionId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            return Ok(new
            {
                status = payment.Status,
                bookingId = payment.BookingId
            });
        }


        [HttpPost("confirm-session/{sessionId}")]
        public async Task<IActionResult> ConfirmSession(string sessionId)
        {
            try
            {
                // 1. Configure Stripe
                var secretKey = _configuration["Stripe:SecretKey"];

                if (string.IsNullOrWhiteSpace(secretKey))
                    return StatusCode(500, "Stripe is not configured.");

                StripeConfiguration.ApiKey = secretKey;

                // 2. Retrieve actual session from Stripe
                var service = new SessionService();
                var session = await service.GetAsync(sessionId);

                if (session == null || session.PaymentStatus != "paid")
                {
                    return Ok(new
                    {
                        status = "Pending",
                        bookingId = 0
                    });
                }

                // 3. Get PaymentId from Stripe metadata
                var paymentIdText =
                    session.Metadata?.GetValueOrDefault("PaymentId");

                if (!int.TryParse(paymentIdText, out int paymentId))
                    return BadRequest("Invalid payment metadata.");

                // 4. Find payment in database
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.Id == paymentId);

                if (payment == null ||
                    payment.StripeSessionId != session.Id ||
                    payment.Amount != session.AmountTotal ||
                    !string.Equals(
                        payment.Currency,
                        session.Currency,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Payment verification failed.");
                }

                // 5. Find booking
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == payment.BookingId);

                if (booking == null)
                    return NotFound("Booking not found.");

                // 6. Update payment status
                payment.Status = "Paid";

                // 7. Get booked seats
                var seats = (booking.Seats ?? string.Empty)
                    .Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries |
                        StringSplitOptions.TrimEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (seats.Count == 0)
                    return BadRequest("No seats found for this booking.");

                // 8. Avoid creating duplicate tickets
                var existingSeats = await _context.Tickets
                    .Where(t => t.BookingId == booking.Id)
                    .Select(t => t.Seats)
                    .ToListAsync();

                var existingSeatSet = new HashSet<string>(
                    existingSeats,
                    StringComparer.OrdinalIgnoreCase);

                // 9. Create missing tickets
                foreach (var seat in seats)
                {
                    if (existingSeatSet.Contains(seat))
                        continue;

                    var ticket = new Ticket
                    {
                        BookingId = booking.Id,
                        Date = booking.Date,
                        Time = booking.Time,
                        Cinemas = booking.Cinemas,
                        Seats = seat,
                        QrToken = Guid.NewGuid().ToString("N"),
                        IsUsed = false
                    };

                    _context.Tickets.Add(ticket);
                    existingSeatSet.Add(seat);
                }

                // 10. Save payment and tickets
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = "Paid",
                    bookingId = payment.BookingId
                });
            }
            catch (StripeException ex)
            {
                return StatusCode(
                    502,
                    "Unable to verify payment with Stripe: " + ex.Message);
            }
        }


    }
}
