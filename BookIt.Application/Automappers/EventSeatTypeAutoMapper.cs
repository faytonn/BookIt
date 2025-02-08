using AutoMapper;
using BookIt.Application.DTOs.EventSeatTypeDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventSeatTypeAutoMapper : Profile
{
    public EventSeatTypeAutoMapper()
    {
        CreateMap<EventDetailSeatType, GetEventSeatTypeDTO>().ReverseMap();
        CreateMap<EventDetailSeatType, UpdateEventSeatTypeDTO>().ReverseMap();
        CreateMap<EventDetailSeatType, CreateEventSeatTypeDTO>().ReverseMap();
    }
}
