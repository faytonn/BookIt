using AutoMapper;
using BookIt.Application.DTOs.SettingDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SettingAutoMapper : Profile
{
    public SettingAutoMapper()
    {
        CreateMap<Setting, GetSettingDTO>()
           .ForMember(x => x.Value, x => x.MapFrom(x => x.SettingDetails.FirstOrDefault() != null ? x.SettingDetails.FirstOrDefault()!.Value : string.Empty)).ReverseMap();
        CreateMap<Setting, UpdateSettingDTO>().ReverseMap();
    }
}
