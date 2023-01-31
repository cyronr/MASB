using MABS.Application.Common.Pagination;
using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Queries.GetAllFacilities
{
    public record GetAllFacilitiesQuery
    (
        PagingParameters PagingParameters
    ) : IRequest<PagedList<FacilityDto>>;
}
