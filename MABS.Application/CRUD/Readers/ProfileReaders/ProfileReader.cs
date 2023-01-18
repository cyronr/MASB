using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ProfileModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.CRUD.Readers.ProfileReaders
{
    internal class ProfileReader : IProfileReader
    {
        private readonly ILogger<ProfileReader> _logger;
        private readonly IProfileRepository _profileRepository;

        public ProfileReader(ILogger<ProfileReader> logger, IProfileRepository profileRepository)
        {
            _logger = logger;
            _profileRepository = profileRepository;
        }

        public async Task<List<Profile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> GetByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting profile by email {email}.");

            var profile = await _profileRepository.GetByEmailAsync(email);
            if (profile is null)
                throw new NotFoundException($"Profile with email = {email} was not found.");

            return profile;
        }

        public async Task<Profile> GetByUUIDAsync(Guid uuid)
        {
            _logger.LogInformation($"Checking if doctor with id = {uuid} exists.");

            var profile = await _profileRepository.GetByUUIDAsync(uuid);
            if (profile is null)
                throw new NotFoundException($"Doctor not found.", $"DoctorId = {uuid}");

            return profile;
        }
    }
}
