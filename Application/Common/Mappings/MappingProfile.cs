using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Language, LanguageDto>();
        CreateMap<User, UserDto>();
        CreateMap<Role, RoleDto>();
        CreateMap<Hashtag, HashtagDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryTranslation, CategoryTranslationDto>();
        CreateMap<Post, PostDto>();
        CreateMap<PostTranslation, PostTranslationDto>();
        CreateMap<PostTranslationDto, PostTranslation>();
    }
}
