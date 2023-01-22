using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.DoctorsBySpecialties
{
    public record DoctorsBySpecialtiesQuery
    (
        PagingParameters PagingParameters,
        List<int> Specialties
    ) : IRequest<PagedList<DoctorDto>>;
}
