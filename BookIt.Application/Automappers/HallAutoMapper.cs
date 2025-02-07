using AutoMapper;
using BookIt.Application.DTOs.HallDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class HallAutoMapper : Profile
{
    public HallAutoMapper()
    {
        CreateMap<Hall, GetHallDTO>().ReverseMap();
        CreateMap<Hall, UpdateHallDTO>().ReverseMap();
        CreateMap<Hall, CreateHallDTO>().ReverseMap();

    }
}
