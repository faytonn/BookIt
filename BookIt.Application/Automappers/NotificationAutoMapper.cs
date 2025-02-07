using AutoMapper;
using BookIt.Application.DTOs.NotificationDetailDTO;
using BookIt.Application.DTOs.NotificationDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class NotificationAutoMapper : Profile
{
    public NotificationAutoMapper()
    {
        CreateMap<Notification, GetNotificationDTO>().ReverseMap();
        CreateMap<Notification, UpdateNotificationDTO>().ReverseMap();
        CreateMap<Notification, CreateNotificationDTO>().ReverseMap();
    }
}
