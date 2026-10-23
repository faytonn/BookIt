using AutoMapper;
using BookIt.Application.DTOs.EventSeatDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventSeatAutoMapper : Profile
{
    public EventSeatAutoMapper()
    {
        CreateMap<EventSeat, GetEventSeatDTO>()
            .ForMember(dest => dest.SeatName, opt => opt.MapFrom(src => src.Seat.SeatName))
            .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Seat.SeatRow))
            .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Seat.SeatColumn))
            .ForMember(dest => dest.SeatTypeName, opt => opt.MapFrom(src => src.Seat.SeatType.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Seat.SeatType.DefaultPrice))
            .ForMember(dest => dest.HeldByMe, opt => opt.Ignore());
    }
}
