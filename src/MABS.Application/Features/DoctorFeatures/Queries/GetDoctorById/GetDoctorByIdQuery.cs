using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetDoctorById
{
    public record GetDoctorByIdQuery
    (
        Guid Id
    ) : IRequest<DoctorDto>;
}
