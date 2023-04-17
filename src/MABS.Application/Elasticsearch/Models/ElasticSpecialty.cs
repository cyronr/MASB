using MABS.Application.Features.DoctorFeatures.Common;

namespace MABS.Application.Elasticsearch.Models
{
    public class ElasticSpecialty
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public SpecialityDto ConvertToSpecialtyDto()
        {
            return new SpecialityDto
            {
                Name = this.Name
            };
        }
    }
}
