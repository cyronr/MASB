using MABS.Application.Common.Exceptions;
using MABS.Application.Repositories;
using MABS.Domain.Models.ProfileModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MABS.Application.Services.Helpers.ProfileHelpers
{
    public class ProfileHelper : IProfileHelper
    {
        private readonly ILogger<ProfileHelper> _logger;
        private readonly IProfileRepository _profileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileHelper(ILogger<ProfileHelper> logger, IProfileRepository profileRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _profileRepository = profileRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Profile> TryGetLoggedProfile()
        {
            var loggerUserUUID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggerUserUUID == null)
                return null;

            return await _profileRepository.GetByUUID(Guid.Parse(loggerUserUUID));
        }

        public async Task CheckProfileAlreadyExists(Profile profile)
        {
            _logger.LogInformation($"Checking if profile with email = {profile.Email} already exists.");

            if (await _profileRepository.GetByEmail(profile.Email) != null)
                throw new AlreadyExistsException($"Profile with Email {profile.Email} already exists.");
        }

        public async Task<Profile> GetByEmail(string email)
        {
            _logger.LogInformation($"Getting profile by email {email}.");

            var profile = await _profileRepository.GetByEmail(email);
            if (profile == null)
                throw new NotFoundException($"Profile with email = {email} was not found.");

            return profile;
        }

        public async Task<Profile> GetByUUID(Guid uuid)
        {
            _logger.LogInformation($"Getting profile by id {uuid}.");

            var profile = await _profileRepository.GetByUUID(uuid);
            if (profile == null)
                throw new NotFoundException($"Profile with id = {uuid} was not found.");

            return profile;
        }
    }
}
