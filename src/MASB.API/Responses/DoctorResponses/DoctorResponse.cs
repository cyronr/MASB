using MABS.Application.Services.DoctorServices.Common;

namespace MASB.API.Responses.DoctorResponses
{
    public record DoctorResponse
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public TitleDto Title { get; set; }
        public List<SpecialityDto> Specialties { get; set; }
    }
}
