using MABS.Application.Features.PatientFeatures.Common;
using MediatR;

namespace MABS.Application.Features.PatientFeatures.Queries.GetPatientByProfile
{
    public record GetPatientByProfileQuery
    (
        Guid Id
    ) : IRequest<PatientDto>;
}
