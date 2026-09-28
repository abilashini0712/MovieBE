namespace MovieTicketBookingSystemBE.DTO.TicketDto
{
    public class TicketDto
    {
        public DateTime Date { get; set; }

        public string Time { get; set; } = string.Empty;

        public string Cinemas { get; set; } = string.Empty;

        public string Seats { get; set; } = string.Empty;

        public int BookingId { get; set; }
    }
}
