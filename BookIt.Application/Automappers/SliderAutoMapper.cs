using AutoMapper;
using BookIt.Application.DTOs.SliderDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class SliderAutoMapper : Profile
{
    public SliderAutoMapper()
    {
        CreateMap<Slider, GetSliderDTO>().ReverseMap();
        CreateMap<Slider, UpdateSliderDTO>().ReverseMap();
        CreateMap<Slider, CreateSliderDTO>().ReverseMap();
    }
}
