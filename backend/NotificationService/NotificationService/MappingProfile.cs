using AutoMapper;
using NotificationService.DTOs;
using NotificationService.Models;

namespace NotificationService.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping from Notification to NotificationDto
            CreateMap<Notification, NotificationDto>();

            // Mapping from NotificationDto to Notification
            CreateMap<NotificationDto, Notification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())  // Id is generated automatically
                .ForMember(dest => dest.IsRead, opt => opt.Ignore())  // IsRead is handled in the business logic
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow));  // Automatically set Timestamp
        }
    }
}
