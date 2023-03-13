using MABS.Application.Common.MessageSenders;
using MABS.Domain.Models.DoctorModels;
using MABS.Infrastructure.Common.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace MABS.Infrastructure.Common.MessageSenders
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly SmtpConfig _smtpConfig;

        public EmailSender(ILogger<EmailSender> logger, IOptions<SmtpConfig> smtpConfig)
        {
            _logger = logger;
            _smtpConfig = smtpConfig.Value;
        }


        public void SendEmail(MailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = _smtpConfig.EnableSsl,
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                Credentials = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password)
            })
            {
                message.From = new MailAddress(_smtpConfig.SendFrom);
                
                try
                {
                    _logger.LogDebug($"Sending email to {message.To}");
                    smtpClient.Send(message);
                }
                catch (Exception e)
                {
                    _logger.LogCritical($"Sending email failed. Details: {e}");
                    throw;
                }
            }
        }
    }
}
