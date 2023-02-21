using MABS.Domain.Models.DoctorModels;
using System.Text.Json;

namespace MABS.Application.Features.DoctorFeatures.Common
{
    public record DoctorDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DoctorStatus.Status Status { get; set; }
        public TitleDto Title { get; set; }
        public List<SpecialityDto> Specialties { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}