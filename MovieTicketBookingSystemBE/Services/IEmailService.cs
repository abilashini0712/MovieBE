using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Services
{
    public interface IEmailService
    {
       
        Task SendContactEmailAsync(Contact contact);
    }
}