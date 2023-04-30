namespace MABS.Domain.Models.ScheduleModels
{
    public class ScheduleEventType
    {
        public enum Type
        {
            Created = 1,
            Deleted = 2
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<ScheduleEvent> Events { get; set; }
    }
}
