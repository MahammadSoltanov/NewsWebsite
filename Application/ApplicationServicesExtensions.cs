using Application.Common.Mappings;
using Application.CQRS.Categories.Commands.CreateCategory;
using Application.CQRS.CategoryTranslations.Commands.CreateCategoryTranslation;
using Application.CQRS.CategoryTranslations.Commands.UpdateCategoryTranslation;
using Application.CQRS.Hashtags.Commands.CreateHashtag;
using Application.CQRS.Hashtags.Commands.UpdateHashtag;
using Application.CQRS.Images.Commands.CreateImage;
using Application.CQRS.Languages.Commands.CreateLanguage;
using Application.CQRS.Languages.Commands.UpdateLanguage;
using Application.CQRS.Posts.Commands.CreatePost;
using Application.CQRS.Posts.Commands.UpdatePost;
using Application.CQRS.PostTranslations.Commands.CreatePostTranslation;
using Application.CQRS.PostTranslations.Commands.UpdatePostTranslation;
using Application.CQRS.Users.Commands.CreateUser;
using Application.CQRS.Users.Commands.UpdateUser;
using AutoMapper;
using FluentValidation;
using Serilog;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServicesExtensions).Assembly));            

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();    


            var mappingConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = mappingConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IValidator<CreateHashtagCommand>, CreateHashtagCommandValidator>();
            services.AddScoped<IValidator<UpdateHashtagCommand>, UpdateHashtagCommandValidator>();
            services.AddScoped<IValidator<CreateLanguageCommand>, CreateLanguageCommandValidator>();
            services.AddScoped<IValidator<UpdateLanguageCommand>, UpdateLanguageCommandValidator>();
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
            services.AddScoped<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
            services.AddScoped<IValidator<CreateCategoryTranslationCommand>, CreateCategoryTranslationCommandValidator>();
            services.AddScoped<IValidator<UpdateCategoryTranslationCommand>, UpdateCategoryTranslationCommandValidator>();
            services.AddScoped<IValidator<CreatePostCommand>, CreatePostCommandValidator>();
            services.AddScoped<IValidator<UpdatePostCommand>, UpdatePostCommandValidator>();
            services.AddScoped<IValidator<CreatePostTranslationCommand>, CreatePostTranslationCommandValidator>();
            services.AddScoped<IValidator<UpdatePostTranslationCommand>, UpdatePostTranslationCommandValidator>();
            services.AddScoped<IValidator<CreateImageCommand>, CreateImageCommandValidator>();

            return services;
        }
    }
}
