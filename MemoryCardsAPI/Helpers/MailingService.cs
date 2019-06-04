using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System;

namespace MemoryCardsAPI.Helpers
{
    public class MailingService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "info@pr42.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync("smtp.yandex.ru", 465, MailKit.Security.SecureSocketOptions.Auto);
                await client.AuthenticateAsync("info@pr42.ru", "pr42!m@il");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}