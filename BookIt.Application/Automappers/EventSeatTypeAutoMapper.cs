using AutoMapper;
using BookIt.Application.DTOs.EventSeatTypeDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventSeatTypeAutoMapper : Profile
{
    public EventSeatTypeAutoMapper()
    {
        CreateMap<EventSeatType, GetEventSeatTypeDTO>().ReverseMap();
        CreateMap<EventSeatType, UpdateEventSeatTypeDTO>().ReverseMap();
        CreateMap<EventSeatType, CreateEventSeatTypeDTO>().ReverseMap();
    }
}
