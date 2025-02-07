using AutoMapper;
using BookIt.Application.DTOs.NewsDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class NewsAutoMapper : Profile
{
    public NewsAutoMapper()
    {
        CreateMap<News, GetNewsDTO>().ReverseMap();
        CreateMap<News, UpdateNewsDTO>().ReverseMap();
        CreateMap<News, CreateNewsDTO>().ReverseMap();
    }
}
