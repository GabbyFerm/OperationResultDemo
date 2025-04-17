using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(assembly);

            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
