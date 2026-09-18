using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystemBE.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int MovieId { get; set; }
        public DateTime? Date { get; set; }
        public string? Time { get; set; } 
        public string? cinemas { get; set; } 
        public int? Tickets { get; set; }
        public string? Seats { get; set; }
        
        [ForeignKey("MovieId")]

        public Movie movie { get; set; }
    }
}
