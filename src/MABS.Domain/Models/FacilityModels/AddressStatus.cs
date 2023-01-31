namespace MABS.Domain.Models.FacilityModels
{
    public class AddressStatus
    {
        public enum Status
        {
            Active = 1,
            Deleted = 2
        }

        public Status Id { get; set; }
        public string Name { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
