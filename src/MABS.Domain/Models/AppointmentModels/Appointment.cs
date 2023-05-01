using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Domain.Models.AppointmentModels;

public class Appointment
{
    public int Id { get; set; }
    public Guid UUID { get; set; }
    public AppointmentStatus.Status StatusId { get; set; }
    public AppointmentStatus Status { get; set; }
    public Patient Patient { get; set; }
    public Schedule Schedule { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int ConfirmationCode { get; set; }

    public List<AppointmentEvent> Events { get; set; }
}
