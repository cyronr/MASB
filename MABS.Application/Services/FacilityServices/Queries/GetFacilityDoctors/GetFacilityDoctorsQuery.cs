using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Queries.GetFacilityDoctors
{
    public record GetFacilityDoctorsQuery
    (
        PagingParameters PagingParameters,
        Guid FacilityId
    ) : IRequest<PagedList<DoctorDto>>;
}
