using AutoMapper;
using BookIt.Application.DTOs.NotificationDTO;
using BookIt.Application.DTOs.SeatDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SeatAutoMapper : Profile
{
    public SeatAutoMapper()
    {
        CreateMap<Seat, GetSeatDTO>()
             .ForMember(dest => dest.SeatTypeName, opt => opt.MapFrom(src => src.SeatType.Name));    

        CreateMap<Seat, GetSeatDTO>().ReverseMap();
        CreateMap<Seat, UpdateSeatDTO>().ReverseMap();
        CreateMap<Seat, CreateSeatDTO>().ReverseMap();
        CreateMap<Seat, CreateBulkSeatDTO>().ReverseMap();
    }
}
