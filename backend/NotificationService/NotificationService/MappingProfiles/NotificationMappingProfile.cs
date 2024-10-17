using AutoMapper;
using NotificationService.Models;
using NotificationService.DTOs;

namespace NotificationService.MappingProfiles
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            // Map from Notification model to NotificationDto and vice versa
            CreateMap<Notification, NotificationDto>().ReverseMap();
        }
    }
}
