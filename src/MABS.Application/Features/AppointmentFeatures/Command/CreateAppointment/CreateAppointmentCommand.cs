using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;

public record CreateAppointmentCommand : IRequest<AppointmentDto>
{
    public Guid PatientId { get; set; }
    public Guid ScheduleId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
}
