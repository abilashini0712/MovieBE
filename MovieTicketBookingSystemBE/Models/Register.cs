
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class Register
    {
        [Key]
        public int RegisterId { get; set; }


        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}

