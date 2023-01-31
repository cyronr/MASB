using MABS.Application.Services.PatientServices.Common;
using MediatR;

namespace MABS.Application.Services.PatientServices.Commands.CreatePatient
{
    public record CreatePatientCommand : IRequest<PatientDto>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Guid ProfileId { get; set; }
    }
}
