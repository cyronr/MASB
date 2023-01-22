using MABS.Application.Services.AuthenticationServices.Common;
using MABS.Application.Services.DoctorServices.Commands.CreateDoctor;
using MediatR;

namespace MABS.Application.Services.AuthenticationServices.RegisterDoctor
{
    public record RegisterDoctorCommand
    (
        string Email,
        string Password,
        string? PhoneNumber,
        CreateDoctorCommand Doctor
    ) : IRequest<AuthenticationResultDto>;
}
