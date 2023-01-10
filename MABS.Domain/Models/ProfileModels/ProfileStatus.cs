namespace MABS.Domain.Models.ProfileModels
{
    public class ProfileStatus
    {
        public enum Status
        {
            Prepared = 1,
            Active = 2,
            Locked = 3,
            Deleted = 4
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Profile> Profiles { get; set; }
    }
}
