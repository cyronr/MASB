using MABS.Application.Common.Pagination;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Commands.AddDoctorToFacility
{
    public record AddDoctorToFacilityCommand(
        PagingParameters PagingParameters,
        Guid FacilityId,
        Guid DoctorId
    ) : IRequest<PagedList<DoctorDto>>;
}
