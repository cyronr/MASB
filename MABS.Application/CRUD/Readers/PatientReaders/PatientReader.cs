using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.CRUD.Readers.PatientReaders
{
    internal class PatientReader : IPatientReader
    {
        private readonly ILogger<PatientReader> _logger;
        private readonly IPatientRepository _patientRepository;

        public PatientReader(ILogger<PatientReader> logger, IPatientRepository patientRepository)
        {
            _logger = logger;
            _patientRepository = patientRepository;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Patient> GetByProfileAsync(Profile profile)
        {
            _logger.LogInformation($"Getting patient with ProfileId = {profile.UUID}.");

            var patient = await _patientRepository.GetByProfileAsync(profile);
            if (patient is null)
                throw new NotFoundException($"Patient not found.", $"ProfileId = {profile.UUID}");

            return patient;
        }

        public async Task<Patient> GetByUUIDAsync(Guid uuid)
        {
            _logger.LogInformation($"Checking if Patient with id = {uuid} exists.");

            var Patient = await _patientRepository.GetByUUIDAsync(uuid);
            if (Patient is null)
                throw new NotFoundException($"Patient not found.", $"PatientId = {uuid}");

            return Patient;
        }
    }
}
