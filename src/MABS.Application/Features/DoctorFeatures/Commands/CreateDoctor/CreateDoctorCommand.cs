using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor
{
    public record CreateDoctorCommand : IRequest<DoctorDto?>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int TitleId { get; set; }
        public List<int> Specialties { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
