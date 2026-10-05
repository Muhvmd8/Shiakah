using Microsoft.AspNetCore.Http;

namespace ServiceAbstraction
{
    public interface IMailingService
    {
        Task SendEmailAsync(string toEmail, string subject, string bodyHtml, IList<IFormFile>? attachments = null);
    }
}
