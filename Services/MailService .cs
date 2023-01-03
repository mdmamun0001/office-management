﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Zdm_management.Settings;

namespace Zdm_management.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest, List<string> Mailtolist)
        {
            InternetAddressList list = new InternetAddressList();
            foreach (var item in Mailtolist)
            {
                list.Add(MailboxAddress.Parse(item));
            }
          //  message.From.Add(new MailboxAddress("CUBES", from));
           // message.To.AddRange(list);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.AddRange(list);
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
              builder.HtmlBody = mailRequest.Body;
             email.Body = builder.ToMessageBody();
             var smtp = new SmtpClient();
             smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
             smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
             await smtp.SendAsync(email);
             smtp.Disconnect(true);
            
        }
    }
}


