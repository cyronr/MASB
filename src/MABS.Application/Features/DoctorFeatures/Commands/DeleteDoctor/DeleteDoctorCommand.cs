using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Commands.DeleteDoctor
{
    public record DeleteDoctorCommand
    (
        Guid Id
    ) : IRequest<DoctorDto>;
}
