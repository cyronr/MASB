using MABS.Application.Common.Pagination;
using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetFacilityById
{
    public record GetFacilityByIdQuery
    (
        Guid Id
    ) : IRequest<FacilityDto>;
}
