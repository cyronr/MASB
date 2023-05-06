using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctor;

public record GetByDoctorQuery(
    Guid DoctorId,
    PagingParameters PagingParameters
) : IRequest<PagedList<AppointmentDto>>;

