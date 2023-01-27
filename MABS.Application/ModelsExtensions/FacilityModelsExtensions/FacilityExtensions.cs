using MABS.Application.Common.Http;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.ModelsExtensions.FacilityModelsExtensions
{
    public static class FacilityExtensions
    {
        public static async Task<Facility> GetByUUIDAsync(this Facility facility, IFacilityRepository facilityRepository, Guid uuid)
        {
            facility = await facilityRepository.GetByUUIDAsync(uuid);
            if (facility is null)
                throw new NotFoundException($"Facility not found.", $"FacilityId = {uuid}");

            return facility;
        }

        public static async Task<Facility> GetWithDoctorsByUUIDAsync(this Facility facility, IFacilityRepository facilityRepository, Guid uuid)
        {
            facility = await facilityRepository.GetWithAllDoctorsByUUIDAsync(uuid);
            if (facility is null)
                throw new NotFoundException($"Facility not found.", $"FacilityId = {uuid}");

            return facility;
        }

        public static async Task CheckTINWithVATRegisterAsync(this Facility facility, IHttpRequester httpRequester)
        {
            string url = $@"https://wl-api.mf.gov.pl//api/search/nip/{facility.TaxIdentificationNumber}?date={DateTime.Now.ToString("yyyy-MM-dd")}";
            HttpResponseMessage response = await httpRequester.HttpGet(url);

            if (!response.IsSuccessStatusCode)
                throw new WrongTaxIdentificationNumberException($"{facility.TaxIdentificationNumber} was not found in VAT Register.");
        }

        public static async Task CheckAlreadyExistsAsync(this Facility facility, IFacilityRepository facilityRepository)
        {
            if (await facilityRepository.GetByTINAsync(facility.TaxIdentificationNumber) is not null)
                throw new AlreadyExistsException($"Facility with Tax Identification Number = {facility.TaxIdentificationNumber} already exists.");
        }
    }
}
