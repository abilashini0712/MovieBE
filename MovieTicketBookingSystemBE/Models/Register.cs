
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class Register
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        
        [Required]
        public string Password { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();
    }
}
