using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.AllDoctors
{
    public record AllDoctorsQuery
    (
        PagingParameters PagingParameters
    ) : IRequest<PagedList<DoctorDto>>;
}
