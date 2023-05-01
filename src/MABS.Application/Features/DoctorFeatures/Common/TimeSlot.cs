namespace MABS.Application.Features.DoctorFeatures.Common;

public record TimeSlot
{
    public Guid ScheduleId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public bool Available { get; set; }

    public TimeSlot(Guid scheduleId, DateOnly date, TimeOnly time)
    {
        this.ScheduleId = scheduleId;
        this.Date = date;
        this.Time = time;
        this.Available = true;
    }
}

