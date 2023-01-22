using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.DoctorById
{
    public record DoctorByIdQuery
    (
        Guid Id
    ) : IRequest<DoctorDto>;
}
