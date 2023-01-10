namespace MABS.Domain.Models.ProfileModels
{
    public class ProfileEventType
    {
        public enum Type
        {
            Created = 1,
            Updated = 2,
            Deleted = 3
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<ProfileEvent> Events { get; set; }
    }
}
