namespace MovieTicketBookingSystemBE.Models
{
    public class Booking
    {
      public int id { get; set; }

        public string Date { get; set; } = string.Empty;

        public string Cinemas { get; set; } = string.Empty;

        public int Tickets { get; set; } 

        public string Seats { get; set; } = string.Empty;

    }
}


