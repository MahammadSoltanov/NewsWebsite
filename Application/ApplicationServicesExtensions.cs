using Application.Common.Mappings;
using Application.CQRS.Hashtags.Commands.CreateHashtag;
using Application.CQRS.Hashtags.Commands.UpdateHashtag;
using Application.CQRS.Languages.Commands.CreateLanguage;
using Application.CQRS.Languages.Commands.UpdateLanguage;
using Application.CQRS.Users.Commands.CreateUser;
using Application.CQRS.Users.Commands.UpdateUser;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
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

            services.AddScoped<IValidator<CreateHashtagCommand>, CreateHashtagCommandValidator>();
            services.AddScoped<IValidator<UpdateHashtagCommand>, UpdateHashtagCommandValidator>();
            services.AddScoped<IValidator<CreateLanguageCommand>, CreateLanguageCommandValidator>();
            services.AddScoped<IValidator<UpdateLanguageCommand>, UpdateLanguageCommandValidator>();
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServicesExtensions).Assembly));
            services.AddSingleton(mapper);
            return services;
        }
    }
}
