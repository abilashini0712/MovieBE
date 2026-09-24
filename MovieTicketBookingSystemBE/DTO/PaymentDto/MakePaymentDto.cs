namespace MovieTicketBookingSystemBE.DTO.PaymentDto
{
    public class MakePaymentDto
    {
        public string name { get; set; } = string.Empty;

        public int number { get; set; } 

        public string date { get; set; } = string.Empty;

        public int cvv { get; set; }
    }
}
