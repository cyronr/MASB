using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Commands.DeleteDoctor
{
    public record DeleteDoctorCommand
    (
        Guid Id
    ) : IRequest<DoctorDto>;
}
