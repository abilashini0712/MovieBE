namespace MovieTicketBookingSystemBE.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public string name { get; set; } = string.Empty;

        public string number { get; set; } = string.Empty;

        public string date { get; set; } = string.Empty;

        public int cvv { get; set; } 
    }
}


