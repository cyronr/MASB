using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.CRUD.Readers.FacilityReaders
{
    public interface IFacilityReader : IReader<Facility>
    {
        Task<Facility> GetFacilityWithDoctorsByUUIDAsync(Guid uuid);
        Task<Country> GetCountryByIdAsync(string id);
        Task<List<Country>> GetAllCountriesAsync();
        Task<List<AddressStreetType>> GetAllStreetTypesAsync();
    }
}
