using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.DictionaryModels;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;

namespace MABS.Application.Services.Helpers.FacilityHelpers
{
    public class FacilityHelper : IFacilityHelper
    {
        private readonly ILogger<FacilityHelper> _logger;
        private readonly IFacilityRepository _facilityRepository;
        public FacilityHelper(ILogger<FacilityHelper> logger, IFacilityRepository facilityRepository)
        {
            _logger = logger;
            _facilityRepository = facilityRepository;
        }

        public async Task<Facility> GetFacilityByUUID(Guid uuid)
        {
            _logger.LogInformation($"Getting facility with id = {uuid}.");

            var facility = await _facilityRepository.GetByUUID(uuid);
            if (facility == null)
                throw new NotFoundException($"Facility with Id = {uuid} was not found.");

            return facility;
        }

        public async Task<Facility> GetFacilityWithDoctorsByUUID(Guid uuid)
        {
            _logger.LogInformation($"Getting facility with id = {uuid} with list of doctors.");

            var facility = await _facilityRepository.GetWithAllDoctorsByUUID(uuid);
            if (facility == null)
                throw new NotFoundException($"Facility with Id = {uuid} was not found.");

            return facility;
        }

        public async Task<Country> GetCountryById(string id)
        {
            _logger.LogInformation($"Getting country with id = {id}.");

            var country = await _facilityRepository.GetCountryById(id);
            if (country == null)
                throw new NotFoundException($"Country with Id = {id} was not found.");

            return country;
        }
        public async Task CheckFacilityAlreadyExists(Facility facility)
        {
            _logger.LogInformation($"Checking if facility with TIN = {facility.TaxIdentificationNumber} already exists.");

            if (await _facilityRepository.GetByTIN(facility.TaxIdentificationNumber) != null)
                throw new AlreadyExistsException($"Facility with Tax Identification Number = {facility.TaxIdentificationNumber} already exists.");
        }

        public async Task CheckAddressAlreadyExists(Address address)
        {
            _logger.LogInformation($"Checking if address ({address.ToString()}) already exists.");
            var existingAddress = await _facilityRepository.GetAddressByProperties(
                    address.StreetName,
                    address.HouseNumber,
                    address.FlatNumber,
                    address.City,
                    address.PostalCode,
                    address.Country);

            if (existingAddress != null && existingAddress.Id != address.Id)
                throw new AlreadyExistsException($"Address already exists.");
        }
    }
}
