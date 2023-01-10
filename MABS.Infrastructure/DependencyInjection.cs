using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Infrastructure.Data;
using MABS.Infrastructure.DataAccess.Common;
using MABS.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MABS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IDbOperation, DbOperation>();
            services.AddScoped<IInternalDbTransaction, InternalDbTransaction>();
            services.AddScoped<ICurrentLoggedProfile, CurrentLoggedProfile>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();

            services.AddDbContext<DataContext>();

            return services;
        }
    }
}
