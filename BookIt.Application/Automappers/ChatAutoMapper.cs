using AutoMapper;
using BookIt.Application.DTOs.ChatDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class ChatAutoMapper : Profile
{
    public ChatAutoMapper()
    {
        CreateMap<Chat, GetChatDTO>().ReverseMap();
        CreateMap<Chat, CreateChatDTO>().ReverseMap();
        CreateMap<Chat, UpdateChatDTO>().ReverseMap();
    }
}