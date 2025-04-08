using Authentification.JWT.DAL.Models;
using Authentification.JWT.Service.Dto;
using AutoMapper;
using SharedDependencies.Dtos;

namespace Authentification.JWT.Service.Mappers;

public class UserToDtoMapper : Profile
{
    public UserToDtoMapper()
    {
        CreateMap<User, UserDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Salt, opt => opt.Ignore());
    }
}
