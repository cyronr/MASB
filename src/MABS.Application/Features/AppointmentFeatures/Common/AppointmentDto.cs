using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.Features.AppointmentFeatures.Common;

public record AppointmentDto
{
    public Guid Id { get; set; }
    public AppointmentStatus.Status Status { get; set; }
    public Patient Patient { get; set; }
    public Schedule Schedule { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int ConfirmationCode { get; set; }
}
