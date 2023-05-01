using MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;

namespace MABS.API.Requests.ScheduleDetails;

public record ScheduleDetailsRequest
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateOnly ValidDateFrom { get; set; }
    public DateOnly ValidDateTo { get; set; }
    public ScheduleOperationType OperationType { get; set; }
}
