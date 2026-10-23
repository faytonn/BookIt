using AutoMapper;
using BookIt.Application.DTOs.NewsDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class NewsAutoMapper : Profile
{
    public NewsAutoMapper()
    {
        // NewsDetails is expected to be pre-filtered to one language by the query
        CreateMap<News, GetNewsDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.NewsDetails.Select(d => d.Title).FirstOrDefault() ?? string.Empty))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.NewsDetails.Select(d => d.Description).FirstOrDefault() ?? string.Empty));

        CreateMap<News, UpdateNewsDTO>().ReverseMap();
        CreateMap<News, CreateNewsDTO>().ReverseMap();
    }
}
