using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystemBE.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string? image { get; set; }
      
        public string? title { get; set; } 
       
        public string? gener { get; set; }
       
        public string? duration { get; set; } 
    }
}
