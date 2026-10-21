
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
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
                return StatusCode(500, "Webhook is not configured.");

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json, signature, webhookSecret);
            }
            catch (StripeException)
            {
                return BadRequest();
            }

            if (stripeEvent.Type ==
                "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;

                if (session?.PaymentStatus == "paid")
                {
                    var paymentIdText =
                        session.Metadata?.GetValueOrDefault("PaymentId");

                    if (int.TryParse(paymentIdText, out int paymentId))
                    {
                        var payment = await _context.Payments
                            .FirstOrDefaultAsync(p => p.Id == paymentId);

                        if (payment != null &&
                            payment.StripeSessionId == session.Id &&
                            payment.Amount == session.AmountTotal &&
                            payment.Currency == session.Currency &&
                            payment.Status != "Paid")
                        {
                            payment.Status = "Paid";
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            return Ok();
        }
    }
}
