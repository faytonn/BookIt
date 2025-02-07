using AutoMapper;
using BookIt.Application.DTOs.NewsDetailsDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class NewsDetailAutoMapper : Profile
{
    public NewsDetailAutoMapper()
    {
        CreateMap<NewsDetail, GetNewsDetailDTO>().ReverseMap();
        CreateMap<NewsDetail, UpdateNewsDetailDTO>().ReverseMap();
        CreateMap<NewsDetail, CreateNewsDetailDTO>().ReverseMap();
    }
}
