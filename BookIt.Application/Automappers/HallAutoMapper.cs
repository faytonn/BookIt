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

        CreateMap<CreateHallDTO, Hall>();
        CreateMap<UpdateHallDTO, Hall>();
        CreateMap<Hall, UpdateHallDTO>();
    }
}
