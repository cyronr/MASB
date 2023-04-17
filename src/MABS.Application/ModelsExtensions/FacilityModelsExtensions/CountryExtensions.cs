using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DictionaryModels;

namespace MABS.Application.ModelsExtensions.FacilityModelsExtensions
{
    public static class CityExtensions
    {
        public async static Task<Country> GetByIdAsync(this Country country, IFacilityRepository facilityRepository, string id)
        {
            //_logger.LogInformation($"Getting country with id = {id}.");
            country = await facilityRepository.GetCountryByIdAsync(id);
            if (country is null)
                throw new NotFoundException($"Country with Id = {id} was not found.");

            return country;
        }
    }
}
