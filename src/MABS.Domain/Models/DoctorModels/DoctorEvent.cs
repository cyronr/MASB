using MABS.Domain.Models.ProfileModels;

namespace MABS.Domain.Models.DoctorModels
{
    public class DoctorEvent
    {
        public int Id { get; set; }
        public DoctorEventType.Type TypeId { get; set; }
        public DoctorEventType Type { get; set; }
        public Doctor Doctor { get; set; }
        public string AddInfo { get; set; } = String.Empty;
        public Profile? CallerProfile { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}