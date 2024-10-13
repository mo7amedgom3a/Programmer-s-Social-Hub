using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SecurityService.DTOs;
using SecurityServiceBackend.DTOs;
using SecurityServiceBackend.Models;

namespace SecurityServiceBackend
{
    public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDTO, UserDTO>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)).ReverseMap();
        CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username)).ReverseMap();
            
        CreateMap<UserDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username)).ReverseMap();

        // map RegisterDTO to UserRegistrationDto
        CreateMap<RegisterDTO, UserRegistrationDto>();
        CreateMap<UserRegistrationDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username)).ReverseMap();

    }
}
}