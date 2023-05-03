using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.Application.Features.DoctorFeatures.Common;

public record DoctorAddressDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public StreetTypeDto StreetType { get; set; }
    public string StreetName { get; set; }
    public int HouseNumber { get; set; }
    public int? FlatNumber { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public CountryDto Country { get; set; }
    public DoctorFacilityDto Facility { get; set; }
}
