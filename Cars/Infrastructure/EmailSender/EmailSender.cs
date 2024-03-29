﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.EmailSender;

public class EmailSender : IEmailSender
{
    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }

    private AuthMessageSenderOptions Options { get; } //set only via Secret Manager

    public Task SendEmailAsync(string email, string subject, string message)
    {
        return Execute(Environment.GetEnvironmentVariable("ASPNETCORE_SENDGRIDKEY"), subject, message, email);
    }

    private Task Execute(string? apiKey, string subject, string message, string email)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("patryk.amsolik@gmail.com", "Potwierdzenie adresu e-mail"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(email));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);

        return client.SendEmailAsync(msg);
    }
}