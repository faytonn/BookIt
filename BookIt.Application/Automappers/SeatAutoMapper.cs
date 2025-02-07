using AutoMapper;
using BookIt.Application.DTOs.NotificationDTO;
using BookIt.Application.DTOs.SeatDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SeatAutoMapper : Profile
{
    public SeatAutoMapper()
    {
        CreateMap<Seat, GetSeatDTO>().ReverseMap();
        CreateMap<Seat, UpdateSeatDTO>().ReverseMap();
        CreateMap<Seat, CreateSeatDTO>().ReverseMap();
    }
}
