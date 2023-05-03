namespace MABS.Application.Features.DoctorFeatures.Common;

public record DoctorFacilityDto
{
    public Guid Id { get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }
    public string TaxIdentificationNumber { get; set; }
}
