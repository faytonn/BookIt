using AutoMapper;
using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SeatTypeAutoMapper : Profile
{
    public SeatTypeAutoMapper()
    {
        CreateMap<SeatType, GetSeatTypeDTO>().ReverseMap();
        CreateMap<SeatType, UpdateSeatTypeDTO>().ReverseMap();
        CreateMap<SeatType, CreateSeatTypeDTO>().ReverseMap();
    }
}
