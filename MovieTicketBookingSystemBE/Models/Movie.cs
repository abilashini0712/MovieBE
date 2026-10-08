using Microsoft.AspNetCore.Components.Forms;

namespace MovieTicketBookingSystemBE.Models
{
    public class Movie
    {
        public int id { get; set; }

        public byte[]? Image { get; set; }

        public string title { get; set; } = string.Empty;

        public string gener { get; set; } = string.Empty;

        public string duration { get; set; } = string.Empty;

        public string show { get; set; } = string.Empty;

       
    }
}



