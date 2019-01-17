using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PearUp.Infrastructure
{
    /// <summary>
    /// Email Helper to send Emails
    /// </summary>
    public class EmailHelper : IEmailHelper, IDisposable
    {
        private readonly EmailServiceConfiguration _emailServiceConfiguration;
        private readonly SmtpClient _smtpClient;
        private bool disposed = false;

        public EmailHelper(IOptions<EmailServiceConfiguration> emailServiceConfiguration, SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
            _emailServiceConfiguration = emailServiceConfiguration.Value;
        }

        private void ConfigureSMTPClient()
        {
            _smtpClient.Host = _emailServiceConfiguration.SMTPHost;
            _smtpClient.Port = _emailServiceConfiguration.SMTPPort;
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential(_emailServiceConfiguration.MailFrom, _emailServiceConfiguration.FromPassword);
        }

        /// <summary>
        /// Sends Email
        /// </summary>
        /// <param name="message">Email body</param>
        public async Task SendAsync(string message)
        {
            ConfigureSMTPClient();
            var mailMessage = GetMailMessage(message);
            await _smtpClient.SendMailAsync(mailMessage);
        }

        private MailMessage GetMailMessage(string message)
        {
            var mailMessage = new MailMessage(_emailServiceConfiguration.MailFrom, _emailServiceConfiguration.MailTo);
            mailMessage.Subject =_emailServiceConfiguration.MailSubject;
            mailMessage.Body = message;
            return mailMessage;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _smtpClient.Dispose();
            }

            disposed = true;
        }
    }
}
