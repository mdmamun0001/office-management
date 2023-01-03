using System.ComponentModel.DataAnnotations;

namespace Zdm_management.Services
{
    public class MailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
