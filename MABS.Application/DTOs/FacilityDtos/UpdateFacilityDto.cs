namespace MABS.Application.DTOs.FacilityDtos
{
    public class UpdateFacilityDto : UpsertFacilityDto
    {
        public Guid Id { get; set; }
    }
}
