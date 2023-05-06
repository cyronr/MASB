using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByAddress;

public record GetByAddressQuery(
    Guid AddressId,
    PagingParameters PagingParameters
) : IRequest<PagedList<AppointmentDto>>;

