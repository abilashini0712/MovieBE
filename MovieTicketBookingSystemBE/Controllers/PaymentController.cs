using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.DTO.BookingDto;
using MovieTicketBookingSystemBE.DTO.PaymentDto;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Payment(MakePaymentDto payment)
        {
            if (payment.number <= 0)
            {
                return BadRequest("Enter the number");
            }
            if (string.IsNullOrWhiteSpace(payment.name))
            {
                return BadRequest("Enter the name");
            }

            var pay = new Payment
            {
                name = payment.name,
                number = payment.number,
                date = payment.date,
                cvv = payment.cvv

            };
            _context.Payments.Add(pay);

            await _context.SaveChangesAsync();

            return Ok(pay);

        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Payment>> GetTopay(int Id)
        {
            var payment = await _context.Payments.FindAsync(Id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

    }
}
