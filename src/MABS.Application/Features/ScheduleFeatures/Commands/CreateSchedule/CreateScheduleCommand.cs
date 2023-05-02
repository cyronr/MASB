using MABS.Application.Features.ScheduleFeatures.Common;
using MediatR;

namespace MABS.Application.Features.ScheduleFeatures.Commands.CreateSchedule;

public record CreateScheduleCommand : IRequest<ScheduleDto>
{
    public Guid DoctorId { get; set; }
    public Guid FacilityId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateOnly ValidDateFrom { get; set; }
    public DateOnly ValidDateTo { get; set; }
}
