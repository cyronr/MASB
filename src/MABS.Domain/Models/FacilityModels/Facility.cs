using System.Text.Json;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Domain.Models.FacilityModels
{
    public class Facility
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public FacilityStatus.Status StatusId { get; set; }
        public FacilityStatus Status { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<FacilityEvent> Events { get; set; }
        public List<Doctor> Doctors { get; set; }
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }

        public override string ToString()
        {
            var facility = new
            {
                Id = UUID,
                ShortName = ShortName,
                Name = Name,
                TaxIdentificationNumber = TaxIdentificationNumber
            };

            return JsonSerializer.Serialize(facility);
        }
    }
}
