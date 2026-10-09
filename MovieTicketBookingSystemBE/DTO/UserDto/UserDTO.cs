
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.DTO.UserDto
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
