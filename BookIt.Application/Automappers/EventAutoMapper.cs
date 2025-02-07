using AutoMapper;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventAutoMapper : Profile
{
    public EventAutoMapper()
    {
        CreateMap<Event, GetEventDTO>().ReverseMap();
        CreateMap<Event, UpdateEventDTO>().ReverseMap();
        CreateMap<Event, CreateEventDTO>().ReverseMap();
    }
}
