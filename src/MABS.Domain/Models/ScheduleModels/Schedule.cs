using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;

namespace MABS.Domain.Models.ScheduleModels;

public class Schedule
{
    public int Id { get; set; }
    public Guid UUID { get; set; }
    public Doctor Doctor { get; set; }
    public Address Address { get; set; }
    public ScheduleStatus.Status StatusId { get; set; }
    public ScheduleStatus Status { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateOnly ValidDateFrom { get; set; }
    public DateOnly ValidDateTo { get; set; }

    public List<ScheduleEvent> Events { get; set; }
}
