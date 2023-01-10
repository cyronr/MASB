using AutoMapper;
using MABS.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Profile = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Services
{
    public abstract class BaseService<T>
    {
        protected readonly ILogger<T> _logger;
        protected readonly IMapper _mapper;
        protected readonly IDbOperation _db;


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        protected Profile LoggedProfile { get; }

        public BaseService(IServicesDependencyAggregate<T> aggregate)
        {
            _mapper = aggregate.mapper;
            _logger = aggregate.logger;
            _db = aggregate.dbOperation;

            _httpContextAccessor = aggregate.httpContextAccessor;
            _currentLoggedProfile = aggregate.currentLoggedProfile;

            LoggedProfile = TryGetLoggedProfile();
        }

        private Profile TryGetLoggedProfile()
        {
            _logger.LogInformation("Try getting logged user.");
            var loggerUserUUID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggerUserUUID == null)
                return null;

            return _currentLoggedProfile.GetByUUID(Guid.Parse(loggerUserUUID));
        }
    }
}
