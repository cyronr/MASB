using System.Text.Json;

namespace MABS.API.Requests.FacilityRequests
{
    public record UpsertAddressRequest
    {
        public string Name { get; set; }
        public int StreetTypeId { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public int? FlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryId { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
