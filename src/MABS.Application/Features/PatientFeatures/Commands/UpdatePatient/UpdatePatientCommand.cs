using MABS.Application.Features.PatientFeatures.Common;
using MediatR;

namespace MABS.Application.Features.PatientFeatures.Commands.UpdatePatient
{
    public record UpdatePatientCommand : IRequest<PatientDto>
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
