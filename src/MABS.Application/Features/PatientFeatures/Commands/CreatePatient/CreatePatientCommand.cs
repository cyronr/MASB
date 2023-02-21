using MABS.Application.Features.PatientFeatures.Common;
using MediatR;

namespace MABS.Application.Features.PatientFeatures.Commands.CreatePatient
{
    public record CreatePatientCommand : IRequest<PatientDto>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Guid ProfileId { get; set; }
    }
}
