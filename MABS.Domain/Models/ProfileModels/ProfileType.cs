namespace MABS.Domain.Models.ProfileModels
{
    public class ProfileType
    {
        public enum Type
        {
            Facility = 1,
            Patient = 2,
            Doctor = 3,
            Admin = 4
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<Profile> Profiles { get; set; }
    }
}
