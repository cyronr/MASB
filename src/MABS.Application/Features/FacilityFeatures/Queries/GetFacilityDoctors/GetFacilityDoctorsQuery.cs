using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetFacilityDoctors
{
    public record GetFacilityDoctorsQuery
    (
        PagingParameters PagingParameters,
        Guid FacilityId
    ) : IRequest<PagedList<DoctorDto>>;
}
