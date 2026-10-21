using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; } = string.Empty;

        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        public string StripeSessionId { get; set; }
            = string.Empty;

        public string Status { get; set; } = "Pending";

        
        public long Amount { get; set; }

        public string Currency { get; set; } = "usd";

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;
    }
}