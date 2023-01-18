using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.Checkers.FacilityCheckers
{
    public interface IFacilityChecker
    {
        Task CheckFacilityAlreadyExistsAsync(Facility facility);
        Task CheckAddressAlreadyExistsAsync(Address address);
        Task CheckTINWithVATRegisterAsync(string taxIdentificationNumber);
    }
}
