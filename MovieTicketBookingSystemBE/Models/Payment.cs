using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; } = string.Empty;

        public int number { get; set; } 

        public string date { get; set; } = string.Empty;

        public int cvv { get; set; }

        public int BookingId { get; set; }

        public Booking? Booking { get; set; }
    }
}


