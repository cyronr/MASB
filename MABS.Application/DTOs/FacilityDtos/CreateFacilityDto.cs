namespace MABS.Application.DTOs.FacilityDtos
{
    public class CreateFacilityDto : UpsertFacilityDto
    {
        public CreateAddressDto Address { get; set; }
    }
}
