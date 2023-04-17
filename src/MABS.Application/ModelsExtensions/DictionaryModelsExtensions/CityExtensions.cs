using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DictionaryModels;

namespace MABS.Application.ModelsExtensions.DictionaryModelsExtensions
{
    public static class CityExtensions
    {
        public async static Task<City> GetByIdAsync(this City city, IDictionaryRepository dictionaryRepository, int id)
        {
            city = await dictionaryRepository.GetCityByIdAsync(id);
            if (city is null)
                throw new NotFoundException($"City with Id = {id} was not found.");

            return city;
        }
    }
}
