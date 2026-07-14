using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Languio.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // ВАЖЛИВО: Впиши сюди свій Gmail та згенерований 16-значний пароль додатка
            var myEmail = "mfxmaxfair@gmail.com";
            var appPassword = "umwm txwk vmaa yvho";

            var fromAddress = new MailAddress(myEmail, "Languio Support");
            var toAddress = new MailAddress(email);

            using var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, appPassword)
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