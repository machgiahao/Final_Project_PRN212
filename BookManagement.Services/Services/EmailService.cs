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
            //var subject = "🎉 Your Order Has Been Confirmed!";
            //var body = $@"
            //    <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; background-color: #f9f9f9;'>
            //        <h2 style='color: #2c3e50;'>Thank you, {emailDto.RecipientName}, for shopping at <span style='color:#e67e22;'>FBook</span>!</h2>
            //        <p>Your order <strong>#{emailDto.OrderId}</strong> has been successfully received.</p>
            //        <table style='width:100%; border-collapse: collapse; margin-top: 20px;'>
            //            <tr>
            //                <td style='padding: 10px; background-color: #ecf0f1;'>Order ID</td>
            //                <td style='padding: 10px;'>{emailDto.OrderId}</td>
            //            </tr>
            //            <tr>
            //                <td style='padding: 10px; background-color: #ecf0f1;'>Recipient Name</td>
            //                <td style='padding: 10px;'>{emailDto.RecipientName}</td>
            //            </tr>
            //            <tr>
            //                <td style='padding: 10px; background-color: #ecf0f1;'>Address</td>
            //                <td style='padding: 10px;'>{emailDto.Address}</td>
            //            </tr>
            //            <tr>
            //                <td style='padding: 10px; background-color: #ecf0f1;'>Phone Number</td>
            //                <td style='padding: 10px;'>{emailDto.PhoneNumber}</td>
            //            </tr>
            //        </table>
            //        <p style='margin-top: 30px;'>We will process and deliver your order as soon as possible.</p>
            //        <p>Best regards,<br/><strong>FBook</strong></p>
            //        <hr/>
            //        <small style='color: #888;'>This is an automated email. Please do not reply.</small>
            //    </div>";

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
