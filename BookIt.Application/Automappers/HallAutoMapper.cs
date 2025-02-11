using AutoMapper;
using BookIt.Application.DTOs.HallDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class HallAutoMapper : Profile
{
    public HallAutoMapper()
    {
        CreateMap<Hall, GetHallDTO>()
             .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : "N/A"));

        CreateMap<CreateHallDTO, Hall>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.HallDetails.FirstOrDefault(d => d.LanguageId == 1).Name));

        CreateMap<UpdateHallDTO, Hall>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.HallDetails.FirstOrDefault(d => d.LanguageId == 1).Name));

        CreateMap<Hall, UpdateHallDTO>()
            .ForMember(dest => dest.HallDetails, opt => opt.Ignore()); 


    }
}
