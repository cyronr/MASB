using MABS.Application.Common.Pagination;
using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Queries.GetFacilityById
{
    public record GetFacilityByIdQuery
    (
        Guid Id
    ) : IRequest<FacilityDto>;
}
