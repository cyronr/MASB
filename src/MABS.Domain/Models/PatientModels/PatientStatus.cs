namespace MABS.Domain.Models.PatientModels
{
    public class PatientStatus
    {
        public enum Status
        {
            Active = 1,
            Deleted = 2
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Patient> Patients { get; set; }
    }
}
