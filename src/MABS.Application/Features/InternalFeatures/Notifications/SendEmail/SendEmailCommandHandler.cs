using MABS.Application.Common.MessageSenders;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace MABS.Application.Features.InternalFeatures.Notifications.SendEmail;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
{
    private readonly ILogger<SendEmailCommandHandler> _logger;
    private readonly IEmailSender _emailSender;
    public SendEmailCommandHandler(ILogger<SendEmailCommandHandler> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }


    public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Sending email to {request.To}");

        var message = new MailMessage("MABS@MABS.PL", request.To);
        message.Subject = request.Subject;
        message.Body = request.Body;
        message.IsBodyHtml = true;

        _emailSender.SendEmail(message);

        return await Task.FromResult(Unit.Value);
    }
}
