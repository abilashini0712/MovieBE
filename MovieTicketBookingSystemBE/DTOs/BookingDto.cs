namespace MovieTicketBookingSystemBE.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }

        public DateTime? Date { get; set; }

        public string? Time { get; set; }

        public string? Cinemas { get; set; }

        public int Tickets { get; set; }

        public string? Seats { get; set; }


    }
}
