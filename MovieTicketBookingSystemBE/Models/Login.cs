using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystemBE.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        public int RegisterId { get; set; }

        public string? UserName { get; set; } 

        public string? Password { get; set; }
        public string? LastName { get; set; }
        [ForeignKey("RegisterId")]
        public Register register { get; set; }
    }
}
