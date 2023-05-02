

using MABS.Domain.Models.AppointmentModels;

namespace MASB.API.Requests.AppointmentResponses;

public record AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid AddressId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public AppointmentStatus.Status Status { get; set; }
}
