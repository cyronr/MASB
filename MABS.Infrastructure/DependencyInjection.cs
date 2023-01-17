using MABS.Application.Common.Interfaces.Authentication;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Infrastructure.Common;
using MABS.Infrastructure.Common.Authentication;
using MABS.Infrastructure.Data;
using MABS.Infrastructure.DataAccess.Common;
using MABS.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddDbContext<DataContext>();

            return services;
        }
    }
}
