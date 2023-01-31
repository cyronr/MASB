using System.Text.Json;

namespace MABS.API.Requests.FacilityRequests
{
    public record UpdateAddressRequest : UpsertAddressRequest
    {
        public Guid Id { get; set; }
    }
}
