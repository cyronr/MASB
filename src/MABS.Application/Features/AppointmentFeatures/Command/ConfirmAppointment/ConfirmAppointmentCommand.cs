using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Command.ConfirmAppointment;

public record ConfirmAppointmentCommand (
    Guid AppointmentId,
    int ConfirmationCode
) : IRequest<AppointmentDto>;
