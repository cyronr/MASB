using MABS.Application.Common.Exceptions;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.Helpers.PatientHelpers
{
    public class PatientHelper : IPatientHelper
    {
        private readonly ILogger<PatientHelper> _logger;
        private readonly IPatientRepository _patientRepository;

        public PatientHelper(ILogger<PatientHelper> logger, IPatientRepository patientRepository)
        {
            _logger = logger;
            _patientRepository = patientRepository;
        }

        public async Task<Patient> GetPatientByProfile(Profile profile)
        {
            _logger.LogInformation($"Getting patient with ProfileId = {profile.UUID}.");

            var patient = await _patientRepository.GetByProfile(profile);
            if (patient == null)
                throw new NotFoundException($"Patient not found.", $"ProfileId = {profile.UUID}");

            return patient;
        }

        public async Task<Patient> GetPatientByUUID(Guid uuid)
        {
            _logger.LogInformation($"Getting patient with id = {uuid}.");

            var patient = await _patientRepository.GetByUUID(uuid);
            if (patient == null)
                throw new NotFoundException($"Patient not found.", $"PatientId = {uuid}");

            return patient;
        }
    }
}
