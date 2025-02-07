using AutoMapper;
using BookIt.Application.DTOs.SettingDetail;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SettingDetailAutoMapper : Profile
{
    public SettingDetailAutoMapper()
    {
        CreateMap<SettingDetail, UpdateSettingDetailDTO>().ReverseMap();
    }
}
