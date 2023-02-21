using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Commands.UpdateDoctor
{
    public record UpdateDoctorCommand : IRequest<DoctorDto>
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int TitleId { get; set; }
        public List<int> Specialties { get; set; }
    }
}
