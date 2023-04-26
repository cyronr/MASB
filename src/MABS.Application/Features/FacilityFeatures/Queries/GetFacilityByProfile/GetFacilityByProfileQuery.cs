using MABS.Application.Common.Pagination;
using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetFacilityByProfile
{
    public record GetFacilityByProfileQuery
    (
        Guid Id
    ) : IRequest<FacilityDto>;
}
