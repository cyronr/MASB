using FluentValidation;
using FluentValidation.AspNetCore;
using MABS.Application.Checkers.FacilityCheckers;
using MABS.Application.Checkers.ProfileCheckers;
using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD;
using MABS.Application.CRUD.Creators.DoctorCreators;
using MABS.Application.CRUD.Creators.FacilityCreators;
using MABS.Application.CRUD.Creators.PatientCreators;
using MABS.Application.CRUD.Creators.ProfileCreators;
using MABS.Application.CRUD.Deleters.DoctorDeleters;
using MABS.Application.CRUD.Deleters.FacilityDeleters;
using MABS.Application.CRUD.Deleters.PatientDeleters;
using MABS.Application.CRUD.Deleters.ProfileDeleters;
using MABS.Application.CRUD.Readers.DoctorReaders;
using MABS.Application.CRUD.Readers.FacilityReaders;
using MABS.Application.CRUD.Readers.PatientReaders;
using MABS.Application.CRUD.Readers.ProfileReaders;
using MABS.Application.CRUD.Updaters.DoctorUpdaters;
using MABS.Application.CRUD.Updaters.FacilityUpdaters;
using MABS.Application.CRUD.Updaters.PatientUpdaters;
using MABS.Application.CRUD.Updaters.ProfileUpdaters;
using MABS.Application.Services;
using MABS.Application.Services.AuthenticationServices;
using MABS.Application.Services.DoctorServices;
using MABS.Application.Services.FacilityServices;
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

            services.AddScoped<IFacilityCRUD, FacilityCRUD>();
            services.AddScoped<IFacilityCreator, FacilityCreator>();
            services.AddScoped<IFacilityReader, FacilityReader>();
            services.AddScoped<IFacilityUpdater, FacilityUpdater>();
            services.AddScoped<IFacilityDeleter, FacilityDeleter>();

            services.AddScoped<IPatientCRUD, PatientCRUD>();
            services.AddScoped<IPatientCreator, PatientCreator>();
            services.AddScoped<IPatientReader, PatientReader>();
            services.AddScoped<IPatientUpdater, PatientUpdater>();
            services.AddScoped<IPatientDeleter, PatientDeleter>();

            services.AddScoped<IProfileCRUD, ProfileCRUD>();
            services.AddScoped<IProfileCreator, ProfileCreator>();
            services.AddScoped<IProfileReader, ProfileReader>();
            services.AddScoped<IProfileUpdater, ProfileUpdater>();
            services.AddScoped<IProfileDeleter, ProfileDeleter>();

            services.AddScoped<IFacilityChecker, FacilityChecker>();
            services.AddScoped<IProfileChecker, ProfileChecker>();

            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped(typeof(IServicesDependencyAggregate<>), typeof(ServicesDependencyAggregate<>));

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Application>();

            services.AddAutoMapper(typeof(Application).Assembly);

            return services;
        }
    }
}
