using MABS.Application.Features.DoctorFeatures.Common;

namespace MABS.Application.Elasticsearch.Models
{
    public record ElasticDoctor
    {
        public long Id { get; set; }
        public Guid UUID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string TitleShortName { get; set; } = null!;
        public string TitleName { get; set; } = null!;
        public List<string> Specalities { get; set; } = new List<string>();


        public DoctorDto ConvertToDoctorDto()
        {
            var specialties = new List<SpecialityDto>();
            this.Specalities.ForEach(s => specialties.Add(new SpecialityDto { Name = s }));

            var title = new TitleDto { Name = this.TitleName, ShortName= this.TitleShortName };

            return new DoctorDto
            {
                Id = this.UUID,
                Firstname = this.FirstName,
                Lastname = this.LastName,
                Title = title,
                Specialties = specialties
            };
        }
    }
}
