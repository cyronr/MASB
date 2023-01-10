namespace MABS.Domain.Models.DoctorModels
{
    public class DoctorEventType
    {
        public enum Type
        {
            Created = 1,
            Updated = 2,
            Deleted = 3
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<DoctorEvent> Events { get; set; }
    }
}
