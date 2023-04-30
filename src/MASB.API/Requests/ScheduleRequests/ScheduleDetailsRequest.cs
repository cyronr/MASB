namespace MABS.API.Requests.ScheduleDetails;

public record ScheduleDetailsRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateTime ValidDateFrom { get; set; }
    public DateTime ValidDateTo { get; set; }
}
