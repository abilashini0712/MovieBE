//namespace MovieTicketBookingSystemBE.DTO.TicketDto
//{
//    public class TicketDto
//    {
//        public string MovieName { get; set; } = string.Empty;
//        public DateTime Date { get; set; }

//        public string Time { get; set; } = string.Empty;

//        public string Cinemas { get; set; } = string.Empty;

//        public string Seats { get; set; } = string.Empty;

//        public int BookingId { get; set; }
//    }
//}



namespace MovieTicketBookingSystemBE.DTO.TicketDto
{
    public class TicketDto
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string MovieName { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Time { get; set; } = string.Empty;

        public string Cinemas { get; set; } = string.Empty;

        // One seat for each ticket
        public string Seats { get; set; } = string.Empty;

        public string QrToken { get; set; } = string.Empty;

        public bool IsUsed { get; set; }
    }
}
