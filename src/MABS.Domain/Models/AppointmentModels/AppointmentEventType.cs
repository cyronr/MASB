namespace MABS.Domain.Models.AppointmentModels
{
    public class AppointmentEventType
    {
        public enum Type
        {
            Created = 1,
            Confirmed = 2,
            Cancelled = 3
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<AppointmentEvent> Events { get; set; }
    }
}
