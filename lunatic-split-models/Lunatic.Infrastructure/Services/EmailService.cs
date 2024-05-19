
using Lunatic.Application.Contracts;
using Lunatic.Application.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Lunatic.Infrastructure.Services {
    public class EmailService : IEmailService {
        private readonly IOptions<EmailSettings> options;

        public EmailService(IOptions<EmailSettings> options) {
            this.options = options;
        }

        public Task<bool> SendEmailAsync(Mail mail) {
            var client = new SendGridClient(options.Value.ApiKey);

            var subject = mail.Subject;
            var body = mail.Body;
            var to = new EmailAddress(mail.To);

            var from = new EmailAddress {
                Email =  options.Value.FromAddress,
                Name = options.Value.FromName
            };

            var message = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = client.SendEmailAsync(message);
            return Task.FromResult(true);
        }
    }
}
