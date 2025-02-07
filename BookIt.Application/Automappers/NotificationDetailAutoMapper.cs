using AutoMapper;
using BookIt.Application.DTOs.NotificationDetailDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class NotificationDetailAutoMapper : Profile
{
    public NotificationDetailAutoMapper()
    {
        CreateMap<NotificationDetail, GetNotificationDetailDTO>().ReverseMap();
        CreateMap<NotificationDetail, CreateEmailVerificationNotificationDTO>().ReverseMap();
        CreateMap<NotificationDetail, CreateSubscriptionNotificationDetailDTO>().ReverseMap();
        CreateMap<NotificationDetail, CreateSuccessfulPurchaseNotificationDTO>().ReverseMap();
        CreateMap<NotificationDetail, UpdateNotificationDetailDTO>().ReverseMap();
        CreateMap<NotificationDetail, BaseEmailNotificationDTO>().ReverseMap();
    }
}
