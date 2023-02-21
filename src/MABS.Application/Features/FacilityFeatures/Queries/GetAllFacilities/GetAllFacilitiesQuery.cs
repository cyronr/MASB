using MABS.Application.Common.Pagination;
using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetAllFacilities
{
    public record GetAllFacilitiesQuery
    (
        PagingParameters PagingParameters
    ) : IRequest<PagedList<FacilityDto>>;
}
