using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetDoctorById
{
    public record GetDoctorByIdQuery
    (
        Guid Id
    ) : IRequest<DoctorDto>;
}
