using MABS.Domain.Models.DictionaryModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IDictionaryRepository
    {
        Task<City?> GetCityByIdAsync(int id);
        Task<List<City>> GetAllCitiesAsync();
    }
}
