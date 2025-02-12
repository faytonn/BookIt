using AutoMapper;
using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SeatTypeAutoMapper : Profile
{
    public SeatTypeAutoMapper()
    {
        CreateMap<SeatType, GetSeatTypeDTO>()
                .ForMember(dest => dest.HallName, 
                opt => opt.MapFrom
                (src => src.Hall != null ? src.Hall.Name : "N/A"))
                .ReverseMap();

        CreateMap<SeatType, UpdateSeatTypeDTO>().ReverseMap();
        CreateMap<SeatType, CreateSeatTypeDTO>().ReverseMap();
    }
}
