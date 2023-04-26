using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Features.PatientFeatures.Commands.CreatePatient;
using MediatR;

namespace MABS.Application.Features.AuthenticationFeatures.Commands.RegisterPatient
{
    public record RegisterPatientCommand
    (
        string Email,
        string Password,
        string? PhoneNumber,
        CreatePatientCommand Patient
    ) : IRequest<AuthenticationResultDto>;
}
