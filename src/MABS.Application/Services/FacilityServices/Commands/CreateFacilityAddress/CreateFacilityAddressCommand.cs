using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Commands.CreateFacilityAddress
{
    public record CreateFacilityAddressCommand : IRequest<FacilityDto>
    {
        public Guid FacilityId { get; set; }
        public string Name { get; set; }
        public int StreetTypeId { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public int? FlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryId { get; set; }
        public Guid? ProfileId { get; set; }
    }
}
