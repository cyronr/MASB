using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Commands.DeleteFacility
{
    public record DeleteFacilityCommand(
        Guid FacilityId
    ) : IRequest<FacilityDto>;
}
