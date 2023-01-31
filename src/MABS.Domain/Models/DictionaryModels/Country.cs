using MABS.Domain.Models.FacilityModels;

namespace MABS.Domain.Models.DictionaryModels
{
    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
