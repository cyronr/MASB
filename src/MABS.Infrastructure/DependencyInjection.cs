using Elasticsearch.Net;
using MABS.Application.Common.Authentication;
using MABS.Application.Common.Http;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Elasticsearch;
using MABS.Infrastructure.Common;
using MABS.Infrastructure.Common.Authentication;
using MABS.Infrastructure.Common.Http;
using MABS.Infrastructure.Data;
using MABS.Infrastructure.DataAccess.Common;
using MABS.Infrastructure.DataAccess.Repositories;
using MABS.Infrastructure.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace MABS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<ConnectionStrings>(configuration.GetSection(ConnectionStrings.SectionName));

            services.AddScoped<IDbOperation, DbOperation>();
            services.AddScoped<IInternalDbTransaction, InternalDbTransaction>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();

            services.AddScoped<IElasticsearchDoctorService, ElasticsearchDoctorService>();

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<IHttpRequester, HttpRequester>();

            services.AddDbContext<DataContext>();

            services.AddSingleton<IElasticClient>(sp =>
            {
                var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
                var settings = new ConnectionSettings(pool)
                    .EnableApiVersioningHeader();

                return new ElasticClient(settings);
            });

            return services;
        }
    }
}
