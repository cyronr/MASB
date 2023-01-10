using FluentValidation;
using FluentValidation.AspNetCore;
using MABS.Application.Services;
using MABS.Application.Services.DoctorServices;
using MABS.Application.Services.FacilityServices;
using MABS.Application.Services.Helpers;
using MABS.Application.Services.Helpers.DoctorHelpers;
using MABS.Application.Services.Helpers.FacilityHelpers;
using MABS.Application.Services.Helpers.PatientHelpers;
using MABS.Application.Services.Helpers.ProfileHelpers;
using MABS.Application.Services.PatientServices;
using MABS.Application.Services.ProfileServices;
using Microsoft.Extensions.DependencyInjection;

namespace MABS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IHelpers, Helpers>();
            services.AddScoped<IDoctorHelper, DoctorHelper>();
            services.AddScoped<IFacilityHelper, FacilityHelper>();
            services.AddScoped<IPatientHelper, PatientHelper>();
            services.AddScoped<IProfileHelper, ProfileHelper>();

            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IProfileService, ProfileService>();

            services.AddScoped(typeof(IServicesDependencyAggregate<>), typeof(ServicesDependencyAggregate<>));

            services.AddFluentValidation();
            services.AddValidatorsFromAssemblyContaining<Application>();

            services.AddAutoMapper(typeof(Application).Assembly);

            return services;
        }
    }
}
