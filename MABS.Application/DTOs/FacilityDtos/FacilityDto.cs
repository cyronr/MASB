namespace MABS.Application.DTOs.FacilityDtos
{
    public class FacilityDto
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
