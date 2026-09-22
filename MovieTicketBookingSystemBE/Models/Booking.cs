using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystemBE.Models
{
    public class Booking
    {
        [Key]
      public int id { get; set; }

        public DateTime Date { get; set; } 

        public string Cinemas { get; set; } = string.Empty;

        public int Tickets { get; set; } 

        public string Seats { get; set; } = string.Empty;

 
      //  public int RegisterId { get; set; }
      //  public Register? Register { get; set; }

    }
}


