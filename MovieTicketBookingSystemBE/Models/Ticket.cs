//using System.ComponentModel.DataAnnotations;

//namespace MovieTicketBookingSystemBE.Models
//{
//    public class Ticket
//    {
//        [Key]

//        public int Id { get; set; }
//        public DateTime Date { get; set; }

//        public string Time { get; set; } = string.Empty;

//        public string Cinemas { get; set; } = string.Empty;

//        public string Seats { get; set; } = string.Empty;

//        //public string Title { get; set; } = string.Empty;

//        public int BookingId { get; set; }

//       public Booking? Booking { get; set; }


//    }
//}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int BookingId { get; set; }

        public string QrToken { get; set; } = string.Empty;

        public bool IsUsed { get; set; } = false;

        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }
    }
}
