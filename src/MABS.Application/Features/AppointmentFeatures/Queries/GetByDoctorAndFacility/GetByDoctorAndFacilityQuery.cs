using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndFacility;

public record GetByDoctorAndFacilityQuery(
    Guid DoctorId,
    Guid FacilityId
) : IRequest<List<AppointmentDto>>;

