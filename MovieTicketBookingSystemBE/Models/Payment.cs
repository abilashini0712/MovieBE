using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystemBE.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int? BookingId { get; set; }
        public string? name { get; set; } 
        public string? number { get; set; } 
        public string? date { get; set; } 
        public string? cvv { get; set; }

        [ForeignKey ("BookingId")]

        public Payment payment { get; set; }
    }
}
