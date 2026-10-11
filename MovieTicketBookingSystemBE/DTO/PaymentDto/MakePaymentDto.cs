namespace MovieTicketBookingSystemBE.DTO.PaymentDto
{
    public class MakePaymentDto
    {
        public string name { get; set; } = string.Empty;

        public string number { get; set; } = string.Empty;

        public string date { get; set; } = string.Empty;

        public int cvv { get; set; }

        public int BookingId { get; set; }
    }
}