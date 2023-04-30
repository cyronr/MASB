using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Domain.Models.ScheduleModels;

public class Schedule
{
    public int Id { get; set; }
    public Guid UUID { get; set; }
    public Doctor Doctor { get; set; }
    public Facility Facility { get; set; }
    public ScheduleStatus.Status StatusId { get; set; }
    public ScheduleStatus Status { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateTime ValidDateFrom { get; set; }
    public DateTime ValidDateTo { get; set; }

    public List<ScheduleEvent> Events { get; set; }
}
