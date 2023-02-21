using MABS.Application.Common.Pagination;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Commands.RemoveDoctorFromFacility
{
    public record RemoveDoctorFromFacilityCommand(
        PagingParameters PagingParameters,
        Guid FacilityId,
        Guid DoctorId
    ) : IRequest<PagedList<DoctorDto>>;
}
