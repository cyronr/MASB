using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Commands.DeleteFacilityAddress
{
    public record DeleteFacilityAddressCommand(
        Guid FacilityId,
        Guid AddressId
    ) : IRequest<FacilityDto>;
}
