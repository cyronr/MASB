using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetTimeSlots;

public record GetTimeSlotsQuery
(
    Guid DoctorId,
    Guid FacilityId
) : IRequest<List<TimeSlot>>;
