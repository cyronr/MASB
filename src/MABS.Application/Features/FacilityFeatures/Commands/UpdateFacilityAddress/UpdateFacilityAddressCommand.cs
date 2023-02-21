using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Commands.UpdateFacilityAddress
{
    public record UpdateFacilityAddressCommand : IRequest<FacilityDto>
    {
        public Guid FacilityId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StreetTypeId { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public int? FlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryId { get; set; }
    }
}
