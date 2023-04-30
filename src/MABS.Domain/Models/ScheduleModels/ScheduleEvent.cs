using MABS.Domain.Models.ProfileModels;

namespace MABS.Domain.Models.ScheduleModels
{
    public class ScheduleEvent
    {
        public int Id { get; set; }
        public ScheduleEventType.Type TypeId { get; set; }
        public ScheduleEventType Type { get; set; }
        public Schedule Schedule { get; set; }
        public string AddInfo { get; set; } = string.Empty;
        public Profile? CallerProfile { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}