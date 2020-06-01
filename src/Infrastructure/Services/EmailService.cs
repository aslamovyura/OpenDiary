using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSettings = EmailConfiguration();
        }

        ///<inheritdoc/>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Open Diary", _emailSettings.EmailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, true);
                    await client.AuthenticateAsync(_emailSettings.EmailAddress, _emailSettings.Password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private EmailSettings EmailConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("emailsettings.json")
                .Build();

            var emailSettings = configuration.GetSection("EmailSettings")
                .Get<EmailSettings>();

            return emailSettings;
        }
    }
}