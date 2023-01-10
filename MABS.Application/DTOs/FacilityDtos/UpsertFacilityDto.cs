using System.Text.Json;

namespace MABS.Application.DTOs.FacilityDtos
{
    public class UpsertFacilityDto
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
