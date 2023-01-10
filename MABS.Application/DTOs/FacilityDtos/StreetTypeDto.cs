using System.Text.Json;

namespace MABS.Application.DTOs.FacilityDtos
{
    public class StreetTypeDto
    {
        public string ShortName { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
