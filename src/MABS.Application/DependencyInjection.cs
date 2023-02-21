using FluentValidation;
using FluentValidation.AspNetCore;
using MABS.Application.Common.AppProfile;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
            services.AddMediatR(typeof(Application).GetTypeInfo().Assembly);

            return services;
        }
    }
}
