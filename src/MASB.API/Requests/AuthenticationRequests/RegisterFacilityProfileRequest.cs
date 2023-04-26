using MABS.API.Requests.FacilityRequests;

namespace MABS.API.Requests.AuthenticationRequests
{
    public record RegisterFacilityProfileRequest : RegisterProfileRequest
    {
        public CreateFacilityRequest Facility { get; set; }
    }
}
