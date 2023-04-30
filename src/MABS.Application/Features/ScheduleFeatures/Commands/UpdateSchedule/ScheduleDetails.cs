namespace MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;

public record ScheduleDetails
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateTime ValidDateFrom { get; set; }
    public DateTime ValidDateTo { get; set; }
}
