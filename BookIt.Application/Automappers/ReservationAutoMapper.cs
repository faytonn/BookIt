using AutoMapper;
using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class ReservationAutoMapper : Profile
{
    public ReservationAutoMapper()
    {
        CreateMap<Reservation, GetReservationDTO>().ReverseMap();
        CreateMap<Reservation, UpdateReservationDTO>().ReverseMap();
        CreateMap<Reservation, CreateReservationDTO>().ReverseMap();
    }
}
