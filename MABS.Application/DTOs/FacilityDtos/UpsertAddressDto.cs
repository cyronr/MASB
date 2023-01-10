using System.Text.Json;

namespace MABS.Application.DTOs.FacilityDtos
{
    public class UpsertAddressDto
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
