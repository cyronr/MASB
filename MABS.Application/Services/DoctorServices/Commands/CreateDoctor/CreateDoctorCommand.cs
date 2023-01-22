using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Commands.CreateDoctor
{
    public record CreateDoctorCommand
    (
        string Firstname,
        string Lastname,
        int TitleId,
        List<int> Specialties
    ) : IRequest<DoctorDto>;
}
