using MABS.Application.Features.ScheduleFeatures.Common;
using MediatR;

namespace MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;

public record UpdateScheduleCommand : IRequest<List<ScheduleDto>>
{
    public Guid DoctorId { get; set; }
    public Guid FacilityId { get; set; }
    public List<ScheduleDetails> Schedules { get; set; }
}
