using MABS.Domain.Models.ProfileModels;

namespace MABS.Domain.Models.FacilityModels
{
    public class FacilityEvent
    {
        public int Id { get; set; }
        public FacilityEventType.Type TypeId { get; set; }
        public FacilityEventType Type { get; set; }
        public Facility Facility { get; set; }
        public string AddInfo { get; set; } = String.Empty;
        public Profile? CallerProfile { get; set; }
    }
}