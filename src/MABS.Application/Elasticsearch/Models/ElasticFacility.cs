using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.Application.Elasticsearch.Models
{
    public class ElasticFacility
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

       
        public FacilityDto ConvertToFacilityDto()
        {
            return new FacilityDto
            {
                Id = this.Id,
                ShortName = this.ShortName,
                Name = this.Name
            };
        }
    }
}
