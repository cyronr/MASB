using FluentValidation;
using FluentValidation.AspNetCore;
using MABS.Application.Checkers.FacilityCheckers;
using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD;
using MABS.Application.CRUD.Creators.FacilityCreators;
using MABS.Application.CRUD.Creators.PatientCreators;
using MABS.Application.CRUD.Deleters.FacilityDeleters;
using MABS.Application.CRUD.Deleters.PatientDeleters;
using MABS.Application.CRUD.Readers.FacilityReaders;
using MABS.Application.CRUD.Readers.PatientReaders;
using MABS.Application.CRUD.Updaters.FacilityUpdaters;
using MABS.Application.CRUD.Updaters.PatientUpdaters;
using MABS.Application.Services;
using MABS.Application.Services.FacilityServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MABS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICurrentLoggedProfile, CurrentLoggedProfile>();

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

            services.AddScoped<IFacilityChecker, FacilityChecker>();

            services.AddScoped<IFacilityService, FacilityService>();

            services.AddScoped(typeof(IServicesDependencyAggregate<>), typeof(ServicesDependencyAggregate<>));

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Application>();

            services.AddAutoMapper(typeof(Application).Assembly);
            services.AddMediatR(typeof(Application).Assembly);

            return services;
        }
    }
}
