namespace MABS.Domain.Models.ScheduleModels
{
    public class ScheduleStatus
    {
        public enum Status
        {
            Active = 1,
            Deleted = 2
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Schedule> Schedules { get; set; }
    }
}
