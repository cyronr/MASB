using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetDoctorsBySpecialties
{
    public record GetDoctorsBySpecialtiesQuery
    (
        PagingParameters PagingParameters,
        List<int> Specialties
    ) : IRequest<PagedList<DoctorDto>>;
}
