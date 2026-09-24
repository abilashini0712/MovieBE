
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Register> Registers { get; set; }
       
        public DbSet<Login> Logins { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Ticket> Tickets { get; set; }


      
    }
}

