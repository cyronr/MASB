using System.Text.Json;
using MABS.Domain.Models.DictionaryModels;

namespace MABS.Domain.Models.FacilityModels
{
    public class Address
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public AddressStatus.Status StatusId { get; set; }
        public AddressStatus Status { get; set; }
        public string Name { get; set; }
        public AddressStreetType.StreetType StreetTypeId { get; set; }
        public AddressStreetType StreetType { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public int? FlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public Country Country { get; set; }
        public Facility Facility { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            var address = new
            {
                Id = UUID,
                Name = Name,
                StreetType = StreetTypeId,
                StreetName = StreetName,
                HouseNumber = HouseNumber,
                FlatNumber = FlatNumber,
                City = City,
                PostalCode = PostalCode,
                Country = Country.Id
            };

            return JsonSerializer.Serialize(address);
        }
    }

}
