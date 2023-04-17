using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.SearchAllDoctors
{
    public record SearchAllDoctors
    (
        PagingParameters PagingParameters,
        string? SearchText,
        int? SpecialtyId,
        int? CityId
    ) : IRequest<PagedList<DoctorDto>>;
}
