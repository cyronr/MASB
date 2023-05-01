namespace MABS.Domain.Models.AppointmentModels
{
    public class AppointmentStatus
    {
        public enum Status
        {
            Prepared = 1,
            Confirmed = 2,
            Cancelled = 3
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
