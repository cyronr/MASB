using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Commands.AddDoctorToFacility
{
    public record AddDoctorToFacilityCommand(
        PagingParameters PagingParameters,
        Guid FacilityId,
        Guid DoctorId
    ) : IRequest<PagedList<DoctorDto>>;
}
