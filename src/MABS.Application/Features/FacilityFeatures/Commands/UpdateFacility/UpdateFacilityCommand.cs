using MediatR;
using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.Application.Features.FacilityFeatures.Commands.UpdateFacility
{
    public record UpdateFacilityCommand : IRequest<FacilityDto>
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
