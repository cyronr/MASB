using MediatR;
using MABS.Application.Services.FacilityServices.Common;

namespace MABS.Application.Services.FacilityServices.Commands.UpdateFacility
{
    public record UpdateFacilityCommand : IRequest<FacilityDto>
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
