using System.Net.Mail;

namespace Zdm_management.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, List<string> Mailtolist);
    }
}
