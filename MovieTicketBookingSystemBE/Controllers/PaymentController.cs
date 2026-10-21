
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
    }
}
