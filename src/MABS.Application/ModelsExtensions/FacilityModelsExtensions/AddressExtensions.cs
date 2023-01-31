using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.ModelsExtensions.FacilityModelsExtensions
{
    public static class AddressExtensions
    {
        public static async Task CheckAlreadyExistsAsync(this Address address, IFacilityRepository facilityRepository)
        {
            var existingAddress = await facilityRepository.GetAddressByPropertiesAsync(
                    address.StreetName,
                    address.HouseNumber,
                    address.FlatNumber,
                    address.City,
                    address.PostalCode,
                    address.Country);

            if (existingAddress is not null && existingAddress.Id != address.Id)
                throw new AlreadyExistsException($"Address already exists.");
        }
    }
}
