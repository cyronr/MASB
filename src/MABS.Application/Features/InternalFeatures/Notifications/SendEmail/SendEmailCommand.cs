using MediatR;

namespace MABS.Application.Features.InternalFeatures.Notifications.SendEmail;

public record SendEmailCommand(
    string Subject,
    string Body,
    string To
) : IRequest; 

