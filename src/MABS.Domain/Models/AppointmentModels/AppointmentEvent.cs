using MABS.Domain.Models.ProfileModels;

namespace MABS.Domain.Models.AppointmentModels
{
    public class AppointmentEvent
    {
        public int Id { get; set; }
        public AppointmentEventType.Type TypeId { get; set; }
        public AppointmentEventType Type { get; set; }
        public Appointment Appointment { get; set; }
        public string AddInfo { get; set; } = string.Empty;
        public Profile? CallerProfile { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}