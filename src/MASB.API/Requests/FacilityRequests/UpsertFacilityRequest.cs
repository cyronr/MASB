using System.Text.Json;

namespace MABS.API.Requests.FacilityRequests
{
    public record UpsertFacilityRequest
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string TaxIdentificationNumber { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
