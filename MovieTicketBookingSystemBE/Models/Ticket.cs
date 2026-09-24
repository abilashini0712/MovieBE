using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class Ticket
    {
        [Key]

        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string Time { get; set; } = string.Empty;

        public string Cinemas { get; set; } = string.Empty;

        public string Seats { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

       // public int BookingId { get; set; }

      //  public Booking? Booking { get; set; }


    }
}
