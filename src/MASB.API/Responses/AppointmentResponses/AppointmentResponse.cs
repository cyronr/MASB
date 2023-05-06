using MABS.API.Responses.AppointmentResponses;
using MABS.API.Responses.FacilityResponses;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Domain.Models.AppointmentModels;
using MASB.API.Responses.DoctorResponses;

namespace MASB.API.Requests.AppointmentResponses;

public record AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public DoctorResponse Doctor { get; set; }
    public AddressDto Address { get; set; }
    public AppointmentFacility Facility { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public AppointmentStatus.Status Status { get; set; }
}
