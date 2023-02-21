using MediatR;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.Application.Features.FacilityFeatures.Commands.CreateFacility
{
    public record CreateFacilityCommand : IRequest<FacilityDto>
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public CreateFacilityAddressCommand Address { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
