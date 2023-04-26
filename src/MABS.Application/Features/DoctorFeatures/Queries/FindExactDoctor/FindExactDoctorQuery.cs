using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.FindExactDoctor
{
    public record FindExactDoctorQuery
    (
        string firstName,
        string lastName,
        List<int> specialtiesIds
    ) : IRequest<DoctorDto>;
}
