using MABS.Application.DTOs.FacilityDtos;

namespace MABS.Application.DTOs.ProfileDtos
{
    public class RegisterFacilityProfileDto : RegisterProfileDto
    {
        public CreateFacilityDto Facility { get; set; }
    }
}
