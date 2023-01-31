using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetDoctorsBySpecialties
{
    public record GetDoctorsBySpecialtiesQuery
    (
        PagingParameters PagingParameters,
        List<int> Specialties
    ) : IRequest<PagedList<DoctorDto>>;
}
