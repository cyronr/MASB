using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.DataAccess.Repositories
{
    public interface IFacilityRepository
    {
        Task<List<Facility>> GetAllAsync();
        Task<Facility?> GetByUUIDAsync(Guid uuid);
        Task<Facility?> GetByTINAsync(string taxIdentificationNumber);
        Task<Facility?> GetWithAllDoctorsByUUIDAsync(Guid uuid);
        void Create(Facility facility);
        void CreateEvent(FacilityEvent facilityEvent);
        Task<Address?> GetAddressByPropertiesAsync(string streetName, int houseNumber, int? flatNumber, string city, string postalCode, Country country);
        Task<Country?> GetCountryByIdAsync(string id);
        Task<List<Country>> GetAllCountriesAsync();
        Task<List<AddressStreetType>> GetAllStreetTypesAsync();
    }
}