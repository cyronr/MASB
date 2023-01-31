namespace MABS.Domain.Models.FacilityModels
{
    public class FacilityEventType
    {
        public enum Type
        {
            Created = 1,
            Updated = 2,
            Activated = 3,
            Deleted = 4
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<FacilityEvent> Events { get; set; }
    }
}
