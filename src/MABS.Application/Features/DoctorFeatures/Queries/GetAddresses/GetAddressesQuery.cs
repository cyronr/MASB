using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetAddresses;

public record GetAddressesQuery
(
    Guid DoctorId
) : IRequest<List<DoctorAddressDto>>;
