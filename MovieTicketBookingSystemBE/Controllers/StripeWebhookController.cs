
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MovieTicketBookingSystemBE.Data;
//using Stripe;
//using Stripe.Checkout;

//namespace MovieTicketBookingSystemBE.Controllers
//{
//    [ApiController]
//    [Route("api/stripe-webhook")]
//    public class StripeWebhookController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IConfiguration _configuration;

//        public StripeWebhookController(
//            ApplicationDbContext context,
//            IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<IActionResult> HandleWebhook()
//        {
//            var json = await new StreamReader(
//                HttpContext.Request.Body).ReadToEndAsync();

//            var signature = Request.Headers["Stripe-Signature"]
//                .ToString();

//            var webhookSecret =
//                _configuration["Stripe:WebhookSecret"];

//            if (string.IsNullOrWhiteSpace(webhookSecret))
//                return StatusCode(500, "Webhook is not configured.");

//            Event stripeEvent;

//            try
//            {
//                stripeEvent = EventUtility.ConstructEvent(
//                    json, signature, webhookSecret);
//            }
//            catch (StripeException)
//            {
//                return BadRequest();
//            }

//            if (stripeEvent.Type ==
//                "checkout.session.completed")
//            {
//                var session = stripeEvent.Data.Object as Session;

//                if (session?.PaymentStatus == "paid")
//                {
//                    var paymentIdText =
//                        session.Metadata?.GetValueOrDefault("PaymentId");

//                    if (int.TryParse(paymentIdText, out int paymentId))
//                    {
//                        var payment = await _context.Payments
//                            .FirstOrDefaultAsync(p => p.Id == paymentId);

//                        if (payment != null &&
//                            payment.StripeSessionId == session.Id &&
//                            payment.Amount == session.AmountTotal &&
//                            payment.Currency == session.Currency &&
//                            payment.Status != "Paid")
//                        {
//                            payment.Status = "Paid";
//                            await _context.SaveChangesAsync();
//                        }
//                    }
//                }
//            }

//            return Ok();
//        }
//    }
//}



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;
using Stripe;
using Stripe.Checkout;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/stripe-webhook")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StripeWebhookController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(
                HttpContext.Request.Body).ReadToEndAsync();

            var signature = Request.Headers["Stripe-Signature"]
                .ToString();

            var webhookSecret =
                _configuration["Stripe:WebhookSecret"];

            if (string.IsNullOrWhiteSpace(webhookSecret))
            {
                return StatusCode(
                    500, "Webhook is not configured.");
            }

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json, signature, webhookSecret);
            }
            catch (StripeException)
            {
                return BadRequest("Invalid Stripe signature.");
            }

            if (stripeEvent.Type != "checkout.session.completed")
            {
                return Ok();
            }

            var session = stripeEvent.Data.Object as Session;

            if (session == null || session.PaymentStatus != "paid")
            {
                return Ok();
            }

            var paymentIdText =
                session.Metadata?.GetValueOrDefault("PaymentId");

            if (!int.TryParse(paymentIdText, out int paymentId))
            {
                return Ok();
            }

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

           
            if (payment.Status != "Paid")
            {
                payment.Status = "Paid";
            }

            
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(
                    b => b.Id == payment.BookingId);

            if (booking == null)
            {
                await _context.SaveChangesAsync();
                return Ok();
            }

            
            var seats = (booking.Seats ?? string.Empty)
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .Where(seat => !string.IsNullOrWhiteSpace(seat))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (seats.Count == 0)
            {
                await _context.SaveChangesAsync();
                return Ok();
            }

            
            var existingSeats = await _context.Tickets
                .Where(t => t.BookingId == booking.Id)
                .Select(t => t.Seats)
                .ToListAsync();

            var existingSeatSet = new HashSet<string>(
                existingSeats,
                StringComparer.OrdinalIgnoreCase);

           
            foreach (var seat in seats)
            {
                if (existingSeatSet.Contains(seat))
                {
                    continue;
                }

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

         
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
