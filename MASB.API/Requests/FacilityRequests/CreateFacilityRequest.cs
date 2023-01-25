namespace MABS.API.Requests.FacilityRequests
{
    public record CreateFacilityRequest : UpsertFacilityRequest
    {
        public CreateAddressRequest Address { get; set; }
    }
}
