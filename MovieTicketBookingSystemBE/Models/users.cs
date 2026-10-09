using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class users
    {

        [Key]
        public int Id { get; set; }
        public string? Email { get; set; } 

        public string? PasswordSalt { get; set; } 

        public string? PasswordHash { get; set; } 

    }
}
