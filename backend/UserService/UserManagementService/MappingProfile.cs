using AutoMapper;
using UserManagementService.DTOs;
using UserManagementService.Models;
namespace UserManagementService.MappingProfiles
{
public class UserProfileMapping : Profile
{
    public UserProfileMapping()
    {
        CreateMap<User, UserProfileDto>()
        .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
        .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Following.Count)).ReverseMap();
        CreateMap<UserProfileCreateDto, User>();
        CreateMap<UserProfileUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UserRegistrationDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));


    }
}

}