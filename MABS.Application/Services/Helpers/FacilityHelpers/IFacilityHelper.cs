using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.Services.Helpers.FacilityHelpers
{
    public interface IFacilityHelper
    {
        Task<Facility> GetFacilityByUUID(Guid uuid);
        Task<Facility> GetFacilityWithDoctorsByUUID(Guid uuid);
        Task<Country> GetCountryById(string id);
        Task CheckFacilityAlreadyExists(Facility facility);
        Task CheckAddressAlreadyExists(Address address);
    }
}
