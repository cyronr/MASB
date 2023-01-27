using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Commands.DeleteFacilityAddress
{
    public record DeleteFacilityAddressCommand(
        Guid FacilityId,
        Guid AddressId
    ) : IRequest<FacilityDto>;
}
