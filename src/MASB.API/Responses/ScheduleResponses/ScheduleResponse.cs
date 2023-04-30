namespace MASB.API.Responses.ScheduleResponses;

public class ScheduleResponse
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateTime ValidDateFrom { get; set; }
    public DateTime ValidDateTo { get; set; }
}
