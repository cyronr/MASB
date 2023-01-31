using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetAllDoctors
{
    public record GetAllDoctorsQuery
    (
        PagingParameters PagingParameters
    ) : IRequest<PagedList<DoctorDto>>;
}
