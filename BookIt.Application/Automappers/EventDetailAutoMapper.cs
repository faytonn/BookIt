using AutoMapper;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventDetailAutoMapper : Profile
{
    public EventDetailAutoMapper()
    {
        CreateMap<EventDetail, GetEventDetailDTO>().ReverseMap();
        CreateMap<EventDetail, UpdateEventDetailDTO>().ReverseMap();
        CreateMap<EventDetail, CreateEventDetailDTO>().ReverseMap();

        CreateMap<UpdateEventDetailDTO, CreateEventDetailDTO>().ReverseMap();


        //CreateMap<EventDetail, GetEventDetailDTO>();
        //CreateMap<CreateEventDetailDTO, EventDetail>();
        //CreateMap<UpdateEventDetailDTO, EventDetail>();
        //CreateMap<EventDetail, UpdateEventDetailDTO>();
    }
}
