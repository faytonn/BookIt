﻿using AutoMapper;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventAutoMapper : Profile
{
    public EventAutoMapper()
    {
        //CreateMap<Event, GetEventDTO>().ReverseMap();
        //CreateMap<Event, UpdateEventDTO>().ReverseMap();
        //CreateMap<Event, CreateEventDTO>().ReverseMap();

        CreateMap<Event, GetEventDTO>()
    .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.GeneralLocation.Name))
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));


        CreateMap<CreateEventDTO, Event>();
        CreateMap<UpdateEventDTO, Event>();
        CreateMap<Event, UpdateEventDTO>(); // For editing
    }
}
