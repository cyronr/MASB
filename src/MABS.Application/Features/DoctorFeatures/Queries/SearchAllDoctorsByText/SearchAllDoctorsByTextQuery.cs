using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.SearchAllDoctorsByText
{
    public record SearchAllDoctorsByTextQuery
    (
        PagingParameters PagingParameters,
        string SearchText
    ) : IRequest<PagedList<DoctorDto>>;
}
