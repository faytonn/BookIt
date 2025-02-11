using AutoMapper;
using BookIt.Application.DTOs.EventSeatTypeDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class EventDetailSeatTypeAutoMapper : Profile
{
    public EventDetailSeatTypeAutoMapper()
    {
        CreateMap<EventDetailSeatType, GetEventDetailSeatTypeDTO>()
                  .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventDetail.EventId));

        CreateMap<CreateEventDetailSeatTypeDTO, EventDetailSeatType>();

        CreateMap<UpdateEventDetailSeatTypeDTO, EventDetailSeatType>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.AdditionalDetails, opt => opt.MapFrom(src => src.AdditionalDetails));
    }
}
