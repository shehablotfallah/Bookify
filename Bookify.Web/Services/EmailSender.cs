﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Bookify.Web.Services;

public class EmailSender : IEmailSender
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly MailSettings _mailSettings;

    public EmailSender(IWebHostEnvironment webHostEnvironment, IOptions<MailSettings> mailSettings)
    {
        _webHostEnvironment = webHostEnvironment;
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailMessage message = new()
        {
            From = new MailAddress(_mailSettings.Email!, _mailSettings.DisplayName),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        message.To.Add(_webHostEnvironment.IsDevelopment() ? "Shehab-dev@outlook.com" : email);

        SmtpClient smtpClient = new(_mailSettings.Host)
        {
            Port = _mailSettings.Port,
            Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(message);

        smtpClient.Dispose();
    }
}
