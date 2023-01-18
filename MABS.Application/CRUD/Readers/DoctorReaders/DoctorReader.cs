using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.CRUD.Readers.DoctorReaders
{
    public class DoctorReader : IDoctorReader
    {
        private readonly ILogger<IDoctorReader> _logger;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorReader(ILogger<IDoctorReader> logger, IDoctorRepository doctorRepository)
        {
            _logger = logger;
            _doctorRepository = doctorRepository;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }

        public async Task<Doctor> GetByUUIDAsync(Guid uuid)
        {
            _logger.LogInformation($"Checking if doctor with id = {uuid} exists.");

            var doctor = await _doctorRepository.GetByUUIDAsync(uuid);
            if (doctor is null)
                throw new NotFoundException($"Doctor not found.", $"DoctorId = {uuid}");

            return doctor;
        }

        public async Task<Title> GetTitleByIdAsync(int id)
        {
            _logger.LogInformation($"Checking if title with id = {id} exists.");

            var title = await _doctorRepository.GetTitleByIdAsync(id);
            if (title is null)
                throw new DictionaryValueNotExistsException("Wrong TitleId.");

            return title;
        }

        public async Task<List<Specialty>> GetSpecialtiesByIdsAsync(List<int> ids)
        {
            _logger.LogInformation($"Checking if specialties with ids = [{string.Join(", ", ids.ToArray())}] exists.");

            var specalties = await _doctorRepository.GetSpecialtiesByIdsAsync(ids);
            if (specalties.Count != ids.Count)
                throw new DictionaryValueNotExistsException("Wrong Specailties.");

            return specalties;
        }

        public async Task<List<Title>> GetAllTitlesAsync()
        {
            return await _doctorRepository.GetAllTitlesAsync();
        }

        public async Task<List<Specialty>> GetAllSpecialtiesAsync()
        {
            return await _doctorRepository.GetAllSpecialtiesAsync();
        }

        public async Task<List<Doctor>> GetBySpecaltiesAsync(List<int> ids)
        {
            return await _doctorRepository.GetBySpecaltiesAsync(ids);
        }
    }
}
