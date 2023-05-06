using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.API.Responses.AppointmentResponses;

public record AppointmentFacility
{
    public Guid Id { get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }
    public string TaxIdentificationNumber { get; set; }
}
