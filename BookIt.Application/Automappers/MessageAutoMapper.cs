using AutoMapper;
using BookIt.Application.DTOs.MessageDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class MessageAutoMapper : Profile
{
    public MessageAutoMapper()
    {
        CreateMap<Message, GetMessageDTO>().ReverseMap();
        CreateMap<Message, CreateMessageDTO>().ReverseMap();
        CreateMap<Message, UpdateMessageDTO>().ReverseMap();
        CreateMap<Message, DisplayMessageDTO>()
            .ForMember(desc => desc.Fullname, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : $"{src.ChatId}"))
            .ForMember(desc => desc.Text, opt => opt.MapFrom(x => x.Text))
            .ForMember(desc => desc.SentAt, opt => opt.MapFrom(x => x.CreatedAt))
            .ForMember(desc => desc.UserId, opt => opt.MapFrom(x => x.UserId))
            .ForMember(desc => desc.ChatId, opt => opt.MapFrom(x => x.ChatId))
            .ReverseMap();
    }
}
