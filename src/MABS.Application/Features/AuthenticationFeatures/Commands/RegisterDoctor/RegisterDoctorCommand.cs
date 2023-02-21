using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MediatR;

namespace MABS.Application.Features.AuthenticationFeatures.Commands.RegisterDoctor
{
    public record RegisterDoctorCommand
    (
        string Email,
        string Password,
        string? PhoneNumber,
        CreateDoctorCommand Doctor
    ) : IRequest<AuthenticationResultDto>;
}
