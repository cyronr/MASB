using MABS.API.Data.Repositories.PatientRepositories;
using MABS.API.Data.Repositories.ProfileRepositories;
using MABS.Application.Repositories;
using MABS.Application.Services.Helpers;
using MABS.Infrastructure.Data;
using MABS.Infrastructure.Data.CurrentLoggedProfile;
using MABS.Infrastructure.Data.DbOperations;
using MABS.Infrastructure.Data.Repositories.DoctorRepositories;
using MABS.Infrastructure.Data.Repositories.FacilityRepositories;
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
