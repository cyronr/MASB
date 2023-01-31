using MediatR;
using MABS.Application.Services.FacilityServices.Common;
using MABS.Application.Services.FacilityServices.Commands.CreateFacilityAddress;

namespace MABS.Application.Services.FacilityServices.Commands.CreateFacility
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
