using AutoMapper;
using MABS.Application.Common.AppProfile;
//using MABS.Application.Common.AppProfileProfile;
using MABS.Application.DataAccess.Common;
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

        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        protected CallerProfile LoggedProfile { get; }

        public BaseService(IServicesDependencyAggregate<T> aggregate)
        {
            _mapper = aggregate.mapper;
            _logger = aggregate.logger;
            _db = aggregate.dbOperation;

            _currentLoggedProfile = aggregate.currentLoggedProfile;

            LoggedProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);
        }
    }
}
