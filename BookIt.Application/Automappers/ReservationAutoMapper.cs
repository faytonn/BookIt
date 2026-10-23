using AutoMapper;
using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class ReservationAutoMapper : Profile
{
    public ReservationAutoMapper()
    {
        CreateMap<Reservation, GetReservationDTO>()
            .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.Event != null ? src.Event.Title : string.Empty))
            .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.ReservationSeats));

        CreateMap<ReservationSeat, GetReservationSeatDTO>()
            .ForMember(dest => dest.SeatName, opt => opt.MapFrom(src => src.Seat != null ? src.Seat.SeatName : string.Empty))
            .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Seat != null ? src.Seat.SeatRow : 0))
            .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Seat != null ? src.Seat.SeatColumn : 0));

        CreateMap<Reservation, UpdateReservationDTO>().ReverseMap();
        CreateMap<Reservation, CreateReservationDTO>().ReverseMap();
    }
}
