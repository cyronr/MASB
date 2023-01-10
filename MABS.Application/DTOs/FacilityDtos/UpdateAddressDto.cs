using System.Text.Json;

namespace MABS.Application.DTOs.FacilityDtos
{
    public class UpdateAddressDto : UpsertAddressDto
    {
        public Guid Id { get; set; }
    }
}
