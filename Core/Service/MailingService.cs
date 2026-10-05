using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using ServiceAbstraction;
using Shared.Settings;

namespace Service
{
    public class MailingService : IMailingService
    {
        private readonly SmtpSettings _mailSettings;

        public MailingService(IOptions<SmtpSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string bodyHtml, IList<IFormFile>? attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Username),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.From.Add(MailboxAddress.Parse(_mailSettings.Username));

            var builder = new BodyBuilder
            {
                HtmlBody = bodyHtml
            };

            if (bodyHtml.Contains("cid:"))
            {
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Logo.png");
                if (File.Exists(logoPath))
                {
                    var image = builder.LinkedResources.Add(logoPath);
                    image.ContentId = Guid.NewGuid().ToString();
                    builder.HtmlBody = builder.HtmlBody.Replace("{{Logo}}", image.ContentId);
                }
            }

            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
