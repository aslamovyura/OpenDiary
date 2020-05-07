using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CustomIdentityApp.Services
{
    public class EmailService : IMessageSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSettings = EmailConfiguration();
        }

        /////<inheritdoc/>
        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    var emailMessage = new MimeMessage();

        //    emailMessage.From.Add(new MailboxAddress("Site administration", "adm.opendiary@gmail.com"));
        //    emailMessage.To.Add(new MailboxAddress("", email));
        //    emailMessage.Subject = subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = message
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync("smtp.gmail.com", 465, true);
        //        await client.AuthenticateAsync("adm.opendiary@gmail.com", "reallyStrongPwd123");
        //        await client.SendAsync(emailMessage);

        //        await client.DisconnectAsync(true);
        //    }
        //}

        ///<inheritdoc/>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Site administration", _emailSettings.EmailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, true);
                await client.AuthenticateAsync(_emailSettings.EmailAddress, _emailSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        private EmailSettings EmailConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("emailsettings.json")
                .Build();

            var mailSettingSection = configuration.GetSection("EmailSettings");
            var mailSettings = mailSettingSection.Get<EmailSettings>();

            return mailSettings;
        }
    }
}