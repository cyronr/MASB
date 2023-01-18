using MABS.Application.Common.Http;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Checkers.FacilityCheckers
{
    public class FacilityChecker : IFacilityChecker
    {
        private readonly ILogger<FacilityChecker> _logger;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IHttpRequester _httpRequester;

        public FacilityChecker(
            ILogger<FacilityChecker> logger, 
            IFacilityRepository facilityRepository,
            IHttpRequester httpRequester)
        {
            _logger = logger;
            _facilityRepository = facilityRepository;
            _httpRequester = httpRequester;
        }


        public async Task CheckAddressAlreadyExistsAsync(Address address)
        {
            _logger.LogInformation($"Checking if address ({address.ToString()}) already exists.");
            var existingAddress = await _facilityRepository.GetAddressByPropertiesAsync(
                    address.StreetName,
                    address.HouseNumber,
                    address.FlatNumber,
                    address.City,
                    address.PostalCode,
                    address.Country);

            if (existingAddress is not null && existingAddress.Id != address.Id)
                throw new AlreadyExistsException($"Address already exists.");
        }

        public async Task CheckFacilityAlreadyExistsAsync(Facility facility)
        {
            _logger.LogInformation($"Checking if facility with TIN = {facility.TaxIdentificationNumber} already exists.");

            if (await _facilityRepository.GetByTINAsync(facility.TaxIdentificationNumber) is not null)
                throw new AlreadyExistsException($"Facility with Tax Identification Number = {facility.TaxIdentificationNumber} already exists.");
        }

        public async Task CheckTINWithVATRegisterAsync(string taxIdentificationNumber)
        {
            _logger.LogInformation($"Checking facility's TIN with VAT Register.");

            string url = $@"https://wl-api.mf.gov.pl//api/search/nip/{taxIdentificationNumber}?date={DateTime.Now.ToString("yyyy-MM-dd")}";
            HttpResponseMessage response = await _httpRequester.HttpGet(url);

            if (!response.IsSuccessStatusCode)
                throw new WrongTaxIdentificationNumberException($"{taxIdentificationNumber} was not found in VAT Register.");
        }
    }
}
