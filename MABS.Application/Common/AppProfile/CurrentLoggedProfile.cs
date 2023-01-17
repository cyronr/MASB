using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.ProfileModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MABS.Application.Common.AppProfile
{
    public class CurrentLoggedProfile : ICurrentLoggedProfile
    {
        private readonly ILogger<CurrentLoggedProfile> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProfileRepository _profileRepository;

        public CurrentLoggedProfile(ILogger<CurrentLoggedProfile> logger, IHttpContextAccessor httpContextAccessor, IProfileRepository profileRepository)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _profileRepository = profileRepository;
        }

        public Profile? GetCurrentLoggedProfile()
        {
            _logger.LogInformation("Try getting logged user.");
            var loggerUserUUID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggerUserUUID == null)
                return null;

            return _profileRepository.GetByUUID(Guid.Parse(loggerUserUUID));
        }
    }
}
