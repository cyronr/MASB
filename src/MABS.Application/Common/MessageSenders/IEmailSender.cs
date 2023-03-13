using System.Net.Mail;

namespace MABS.Application.Common.MessageSenders
{
    public interface IEmailSender
    {
        void SendEmail(MailMessage message);
    }
}
