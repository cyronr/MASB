using AutoMapper;
using MABS.Application.DataAccess.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services
{
    public interface IServicesDependencyAggregate<T>
    {
        public IMapper mapper { get; }
        public IHttpContextAccessor httpContextAccessor { get; }
        public ILogger<T> logger { get; }
        public IDbOperation dbOperation { get; }
        public ICurrentLoggedProfile currentLoggedProfile { get; }

    }

    public class ServicesDependencyAggregate<T> : IServicesDependencyAggregate<T>
    {
        public ILogger<T> logger { get; }
        public IMapper mapper { get; }
        public IHttpContextAccessor httpContextAccessor { get; }
        public IDbOperation dbOperation { get; }
        public ICurrentLoggedProfile currentLoggedProfile { get; }

        public ServicesDependencyAggregate(
            ILogger<T> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, 
            IDbOperation dbOperation,
            ICurrentLoggedProfile currentLoggedProfile
            )
        {
            this.logger = logger;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.dbOperation = dbOperation;
            this.currentLoggedProfile = currentLoggedProfile;
        }
    }
}
