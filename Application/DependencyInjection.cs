using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly; // Get current assembly (Application layer)

            services.AddMediatR(assembly); // Registers all MediatR handlers (Commands/Queries)
            services.AddAutoMapper(assembly); // Registers AutoMapper profiles
            services.AddValidatorsFromAssembly(assembly); // Registers all FluentValidation validators

            // Register pipeline behaviors 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // Handles validation
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)); // Handles request logging

            return services;
        }
    }
}
