namespace MASB.API.Requests.DoctorRequests
{
    public record UpdateDoctorRequest : UpsertDoctorRequest
    {
        public Guid Id { get; set; }
    }
}
