using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.CRUD.Readers.FacilityReaders
{
    internal class FacilityReader : IFacilityReader
    {
        private readonly ILogger<FacilityReader> _logger;
        private readonly IFacilityRepository _facilityRepository;

        public FacilityReader(ILogger<FacilityReader> logger, IFacilityRepository facilityRepository)
        {
            _logger = logger;
            _facilityRepository = facilityRepository;
        }

        public async Task<List<Facility>> GetAllAsync()
        {
            return await _facilityRepository.GetAllAsync();
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            return await _facilityRepository.GetAllCountriesAsync();
        }

        public async Task<List<AddressStreetType>> GetAllStreetTypesAsync()
        {
            return await _facilityRepository.GetAllStreetTypesAsync();
        }

        public async Task<Facility> GetByUUIDAsync(Guid uuid)
        {
            _logger.LogInformation($"Checking if facility with id = {uuid} exists.");

            var facility = await _facilityRepository.GetByUUIDAsync(uuid);
            if (facility is null)
                throw new NotFoundException($"Facility not found.", $"FacilityId = {uuid}");

            return facility;
        }

        public async Task<Country> GetCountryByIdAsync(string id)
        {
            _logger.LogInformation($"Getting country with id = {id}.");

            var country = await _facilityRepository.GetCountryByIdAsync(id);
            if (country is null)
                throw new NotFoundException($"Country with Id = {id} was not found.");

            return country;
        }

        public async Task<Facility> GetFacilityWithDoctorsByUUIDAsync(Guid uuid)
        {
            _logger.LogInformation($"Getting facility with id = {uuid} with list of doctors.");

            var facility = await _facilityRepository.GetWithAllDoctorsByUUIDAsync(uuid);
            if (facility is null)
                throw new NotFoundException($"Facility with Id = {uuid} was not found.");

            return facility;
        }
    }
}
