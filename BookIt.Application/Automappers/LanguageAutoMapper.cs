using AutoMapper;
using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class LanguageAutoMapper : Profile
{
    public LanguageAutoMapper()
    {
        CreateMap<Language, GetLanguageDTO>().ReverseMap();
    }
}
