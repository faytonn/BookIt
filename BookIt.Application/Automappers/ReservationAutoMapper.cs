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
            .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.ReservationSeats))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User != null ? (src.User.FirstName + " " + src.User.LastName).Trim() : string.Empty))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.User != null ? src.User.PhoneNumber : null))
            .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.User != null ? src.User.Address : null))
            .ForMember(dest => dest.CustomerEmailConfirmed, opt => opt.MapFrom(src => src.User != null && src.User.EmailConfirmed))
            .ForMember(dest => dest.CustomerSince, opt => opt.MapFrom(src => src.User != null ? src.User.CreatedAt : (DateTime?)null))
            .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Event != null ? src.Event.EventDate : default))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Event != null && src.Event.GeneralLocation != null ? src.Event.GeneralLocation.Name : string.Empty))
            .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Event != null && src.Event.Hall != null ? src.Event.Hall.Name : string.Empty))
            .ForMember(dest => dest.RefundAmount, opt => opt.MapFrom(src => src.CancellationRefund != null ? src.CancellationRefund.RefundAmount : (decimal?)null))
            .ForMember(dest => dest.IsRefunded, opt => opt.MapFrom(src => src.CancellationRefund != null && src.CancellationRefund.IsRefunded))
            .ForMember(dest => dest.CancelledAt, opt => opt.MapFrom(src => src.CancellationRefund != null ? src.CancellationRefund.CancellationDate : (DateTime?)null));

        CreateMap<ReservationSeat, GetReservationSeatDTO>()
            .ForMember(dest => dest.SeatName, opt => opt.MapFrom(src => src.Seat != null ? src.Seat.SeatName : string.Empty))
            .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Seat != null ? src.Seat.SeatRow : 0))
            .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Seat != null ? src.Seat.SeatColumn : 0))
            .ForMember(dest => dest.SeatType, opt => opt.MapFrom(src => src.Seat != null && src.Seat.SeatType != null ? src.Seat.SeatType.Name : string.Empty));

        CreateMap<Reservation, UpdateReservationDTO>().ReverseMap();
        CreateMap<Reservation, CreateReservationDTO>().ReverseMap();
    }
}
