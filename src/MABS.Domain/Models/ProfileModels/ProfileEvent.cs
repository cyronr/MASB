namespace MABS.Domain.Models.ProfileModels
{
    public class ProfileEvent
    {
        public int Id { get; set; }
        public ProfileEventType.Type TypeId { get; set; }
        public ProfileEventType Type { get; set; }
        public Profile Profile { get; set; }
        public int ProfileId { get; set; }
        public string AddInfo { get; set; } = string.Empty;
        public Profile CallerProfile { get; set; }
        public int? CallerProfileId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}