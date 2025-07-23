using BookManagement.Services.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using BookManagement.Services.DTOs.Mail;

namespace BookManagement.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmail(EmailDto emailDto)
        {        
            var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_config["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(
                    _config["EmailSettings:SenderEmail"],
                    _config["EmailSettings:SenderPassword"]
                ),
                EnableSsl = true,
            };
            var mail = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:SenderEmail"], "FBook"),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true,
            };

            mail.To.Add(emailDto.ToEmail);

            await smtpClient.SendMailAsync(mail);
        }
    }
}
