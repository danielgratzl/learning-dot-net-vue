using System.Net.NetworkInformation;
using WebApi.Models;
using WebApi.Repositiories;
using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace WebApi.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly IConfiguration _config;

        public SendGridEmailService(SendGridClient client, IConfiguration config) {
            _client = client;
            _config = config;
        }

        public void Send(string email, string pin) {
            var from = new EmailAddress(_config["Email:Sender"], _config["Email:Name"]);
            var subject = "Authentication request";
            var to = new EmailAddress(email);
            var plainTextContent = "Your pin: " + pin;
            var htmlContent = "<h2>Your pin</h2><br><strong>"+pin+"</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            _client.SendEmailAsync(msg);
        }
    }
}