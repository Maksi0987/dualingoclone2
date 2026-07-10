using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Languio.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        // Впроваджуємо IConfiguration для доступу до секретів
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Беремо дані з секретів, а не з коду
            var fromEmail = _config["EmailSettings:SenderEmail"];
            var fromPassword = _config["EmailSettings:SenderPassword"];

            var fromAddress = new MailAddress(fromEmail, "Languio App");
            var toAddress = new MailAddress(email);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}