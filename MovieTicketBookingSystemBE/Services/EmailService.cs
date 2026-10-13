using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MovieTicketBookingSystemBE.Models;

namespace MovieTicketBookingSystemBE.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _settings;

        public EmailService(IOptions<EmailSetting> options)
        {
            _settings = options.Value;
        }

        public async Task SendContactEmailAsync(Contact contact)
        {
            var message = new MimeMessage();

            
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

       
            message.To.Add(MailboxAddress.Parse(_settings.ReceiverEmail));

            
            message.ReplyTo.Add(new MailboxAddress(contact.Name, contact.Email));

            message.Subject = $"[Contact Form] {contact.Subject}";

            
            message.Body = new TextPart("plain")
            {
                Text =
                    $"New contact message received\n\n" +
                    $"Name:    {contact.Name}\n" +
                    $"Email:   {contact.Email}\n" +
                    $"Subject: {contact.Subject}\n" +
                    $"Date:    {contact.CreatedAt:yyyy-MM-dd HH:mm} UTC\n\n" +
                    $"Message:\n{contact.Message}"
            };

            using var client = new SmtpClient();

           
            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort,
                SecureSocketOptions.StartTls);

           
            await client.AuthenticateAsync(_settings.Username, _settings.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}