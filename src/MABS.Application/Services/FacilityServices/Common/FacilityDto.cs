namespace MABS.Application.Services.FacilityServices.Common
{
    public record FacilityDto
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
