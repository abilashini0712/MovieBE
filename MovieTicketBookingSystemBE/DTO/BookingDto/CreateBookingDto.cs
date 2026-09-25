namespace MovieTicketBookingSystemBE.DTO.BookingDto
{
    public class CreateBookingDto
    {
        public DateTime Date { get; set; }

        public string Time { get; set; } = string.Empty;

        public string Cinemas { get; set; } = string.Empty;

        public int Tickets { get; set; }

        public string Seats { get; set; } = string.Empty;


        public int RegisterId { get; set; } 
    }
}

