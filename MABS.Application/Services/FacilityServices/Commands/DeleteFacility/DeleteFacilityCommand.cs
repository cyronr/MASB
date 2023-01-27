using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Commands.DeleteFacility
{
    public record DeleteFacilityCommand(
        Guid FacilityId
    ) : IRequest<FacilityDto>;
}
