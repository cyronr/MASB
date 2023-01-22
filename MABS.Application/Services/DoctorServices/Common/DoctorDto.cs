using System.Text.Json;

namespace MABS.Application.Services.DoctorServices.Common
{
    public record DoctorDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public TitleDto Title { get; set; }
        public List<SpecialityDto> Specialties { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}