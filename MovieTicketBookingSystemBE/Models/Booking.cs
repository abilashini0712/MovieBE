using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystemBE.Models
{
    public class Booking
    {
        [Key]
      public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; } = string.Empty;

        public string Cinemas { get; set; } = string.Empty;

        public int Tickets { get; set; } 

        public string Seats { get; set; } = string.Empty;


        public int RegisterId { get; set; } 

       public Register? Register { get; set; }

        //public ICollection<Ticket> tickets { get; set; } = new List<Ticket>();
    }
}


