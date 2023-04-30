using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.Features.ScheduleFeatures.Common;

public class ScheduleDto
{
    public Guid Id { get; set; }
    public Doctor Doctor { get; set; }
    public Facility Facility { get; set; }
    public ScheduleStatus.Status Status { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int AppointmentDuration { get; set; }
    public DateTime ValidDateFrom { get; set; }
    public DateTime ValidDateTo { get; set; }
}
