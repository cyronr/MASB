using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ProfileModels;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Checkers.ProfileCheckers
{
    public class ProfileChecker : IProfileChecker
    {
        private readonly ILogger<ProfileChecker> _logger;
        private readonly IProfileRepository _profileRepository;

        public ProfileChecker(ILogger<ProfileChecker> logger, IProfileRepository profileRepository)
        {
            _logger = logger;
            _profileRepository = profileRepository;
        }


        public async Task CheckProfileAlreadyExistsAsync(Profile profile)
        {
            _logger.LogInformation($"Checking if profile with email = {profile.Email} already exists.");

            if (await _profileRepository.GetByEmailAsync(profile.Email) is not null)
                throw new AlreadyExistsException($"Profile with Email {profile.Email} already exists.");
        }
    }
}
