namespace MABS.API.Requests.ScheduleDetails;

public record UpdateScheduleRequest
{
    public Guid DoctorId { get; set; }
    public Guid FacilityId { get; set; }
    public List<ScheduleDetailsRequest> Schedules { get; set; }
}
