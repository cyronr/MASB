namespace MABS.Domain.Models.FacilityModels
{
    public class AddressStreetType
    {
        public enum StreetType
        {
            Street = 1,
            Avenue = 2
        }

        public StreetType Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; } = String.Empty;

        public List<Address> Addresses { get; set; }
    }
}
