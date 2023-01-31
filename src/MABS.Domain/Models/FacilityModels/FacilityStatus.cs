namespace MABS.Domain.Models.FacilityModels
{
    public class FacilityStatus
    {
        public enum Status
        {
            Prepared = 1,
            Active = 2,
            Deleted = 3
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Facility> Facilities { get; set; }
    }
}
