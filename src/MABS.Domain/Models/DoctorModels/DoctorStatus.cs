namespace MABS.Domain.Models.DoctorModels
{
    public class DoctorStatus
    {
        public enum Status
        {
            Active = 1,
            Deleted = 2
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Doctor> Doctors { get; set; }
    }
}
