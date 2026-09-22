using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystemBE.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }


        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        
        public int RegisterId { get; set; }

        public Register? Register { get; set; }
        
      
    }
}
