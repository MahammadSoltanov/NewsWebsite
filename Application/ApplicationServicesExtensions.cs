using Application.Common.Mappings;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            var mappingConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = mappingConfiguration.CreateMapper();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServicesExtensions).Assembly));
            services.AddSingleton(mapper);
            return services;
        }
    }
}
