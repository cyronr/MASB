using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndAddress;

public record GetByDoctorAndAddressQuery(
    Guid DoctorId,
    Guid AddressId
) : IRequest<List<AppointmentDto>>;

