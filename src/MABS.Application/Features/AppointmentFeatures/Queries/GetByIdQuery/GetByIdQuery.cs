using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByIdQuery;

public record GetByIdQuery(
    Guid Id
) : IRequest<AppointmentDto>;
