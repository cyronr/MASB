using MABS.Domain.Models.ProfileModels;

namespace MABS.Domain.Models.PatientModels
{
    public class PatientEvent
    {
        public int Id { get; set; }
        public PatientEventType.Type TypeId { get; set; }
        public PatientEventType Type { get; set; }
        public Patient Patient { get; set; }
        public string AddInfo { get; set; } = string.Empty;
        public Profile? CallerProfile { get; set; }
    }
}