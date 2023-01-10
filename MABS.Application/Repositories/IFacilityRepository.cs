using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.Repositories
{
    public interface IFacilityRepository
    {
        Task<List<Facility>> GetAll();
        Task<Facility> GetByUUID(Guid uuid);
        Task<Facility> GetByTIN(string taxIdentificationNumber);
        Task<Facility> GetWithAllDoctorsByUUID(Guid uuid);
        void Create(Facility facility);
        void CreateEvent(FacilityEvent facilityEvent);
        Task<Address> GetAddressByProperties(string streetName, int houseNumber, int? flatNumber, string city, string postalCode, Country country);
        Task<Country> GetCountryById(string id);
        Task<List<Country>> GetAllCountries();
        Task<List<AddressStreetType>> GetAllStreetTypes();
    }
}