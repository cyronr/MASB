using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Commands.UpdateDoctor
{
    public record UpdateDoctorCommand
    (
        Guid Id,
        string Firstname,
        string Lastname,
        int TitleId,
        List<int> Specialties
    ) : IRequest<DoctorDto>;
}
