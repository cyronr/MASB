using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetAllDoctors
{
    public record AllDoctorsQuery
    (
        PagingParameters PagingParameters
    ) : IRequest<PagedList<DoctorDto>>;
}
