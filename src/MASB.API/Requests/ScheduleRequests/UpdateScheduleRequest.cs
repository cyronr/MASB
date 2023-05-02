namespace MABS.API.Requests.ScheduleRequests;

public record UpdateScheduleRequest : UpsertScheduleRequest
{
    public Guid Id { get; set; }
}
