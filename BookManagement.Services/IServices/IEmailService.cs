﻿using BookManagement.Services.DTOs.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto emailDto);
    }
}
