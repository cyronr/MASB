namespace MABS.Domain.Models.PatientModels
{
    public class PatientEventType
    {
        public enum Type
        {
            Created = 1,
            Updated = 2,
            Deleted = 3
        }

        public Type Id { get; set; }
        public string Name { get; set; }

        public List<PatientEvent> Events { get; set; }
    }
}
