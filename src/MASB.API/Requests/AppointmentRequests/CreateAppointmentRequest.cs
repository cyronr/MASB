namespace MASB.API.Requests.AppointmentRequests;

public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid ScheduleId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
}
