using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacility;
using MediatR;

namespace MABS.Application.Features.AuthenticationFeatures.Commands.RegisterFacility
{
    public record RegisterFacilityCommand
    (
        string Email,
        string Password,
        string? PhoneNumber,
        CreateFacilityCommand Facility
    ) : IRequest<AuthenticationResultDto>;
}
