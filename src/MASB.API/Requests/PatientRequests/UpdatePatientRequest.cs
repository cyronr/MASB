namespace MABS.API.Requests.PatientRequests
{
    public record UpdatePatientRequest : UpsertPatientRequest
    {
        public Guid Id { get; set; }
    }
}
