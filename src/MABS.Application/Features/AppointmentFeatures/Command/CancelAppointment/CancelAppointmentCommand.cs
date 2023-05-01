using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Command.CancelAppointment;

public record CancelAppointmentCommand (
    Guid AppointmentId
) : IRequest<AppointmentDto>;
