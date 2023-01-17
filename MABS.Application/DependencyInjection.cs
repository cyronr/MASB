using FluentValidation;
using FluentValidation.AspNetCore;
using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD;
using MABS.Application.CRUD.Creators.DoctorCreator;
using MABS.Application.CRUD.Deleters.DoctorDeleter;
using MABS.Application.CRUD.Readers.DoctorReader;
using MABS.Application.CRUD.Updaters.DoctorUpdater;
using MABS.Application.Services;
using MABS.Application.Services.AuthenticationServices;
using MABS.Application.Services.DoctorServices;
using MABS.Application.Services.FacilityServices;
using MABS.Application.Services.Helpers;
using MABS.Application.Services.Helpers.DoctorHelpers;
using MABS.Application.Services.Helpers.FacilityHelpers;
using MABS.Application.Services.Helpers.PatientHelpers;
using MABS.Application.Services.Helpers.ProfileHelpers;
using MABS.Application.Services.PatientServices;
using Microsoft.Extensions.DependencyInjection;

namespace MABS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICurrentLoggedProfile, CurrentLoggedProfile>();

            services.AddScoped<IDoctorCRUD, DoctorCRUD>();
            services.AddScoped<IDoctorCreator, DoctorCreator>();
            services.AddScoped<IDoctorReader, DoctorReader>();
            services.AddScoped<IDoctorUpdater, DoctorUpdater>();
            services.AddScoped<IDoctorDeleter, DoctorDeleter>();

            services.AddScoped<IHelpers, Helpers>();
            services.AddScoped<IDoctorHelper, DoctorHelper>();
            services.AddScoped<IFacilityHelper, FacilityHelper>();
            services.AddScoped<IPatientHelper, PatientHelper>();
            services.AddScoped<IProfileHelper, ProfileHelper>();

            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped(typeof(IServicesDependencyAggregate<>), typeof(ServicesDependencyAggregate<>));

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Application>();

            services.AddAutoMapper(typeof(Application).Assembly);

            return services;
        }
    }
}
