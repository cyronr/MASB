namespace MABS.API.Requests.ScheduleRequests;

public record UpsertScheduleRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateOnly ValidDateFrom { get; set; }
    public DateOnly ValidDateTo { get; set; }
}
