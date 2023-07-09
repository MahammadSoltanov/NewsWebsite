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
    }
}
