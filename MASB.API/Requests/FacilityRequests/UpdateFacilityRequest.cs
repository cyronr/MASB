namespace MABS.API.Requests.FacilityRequests
{
    public record UpdateFacilityRequest : UpsertFacilityRequest
    {
        public Guid Id { get; set; }
    }
}
