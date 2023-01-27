using FluentValidation;
using FluentValidation.AspNetCore;
using MABS.Application.Common.AppProfile;
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

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Application>();

            services.AddAutoMapper(typeof(Application).Assembly);
            services.AddMediatR(typeof(Application).Assembly);

            return services;
        }
    }
}
