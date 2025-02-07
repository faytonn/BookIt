using AutoMapper;
using BookIt.Application.DTOs.GeneralLocationDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class GeneralLocationAutoMapper : Profile
{
    public GeneralLocationAutoMapper()
    {
        CreateMap<GeneralLocation, GetGeneralLocationDTO>().ReverseMap();
        CreateMap<GeneralLocation, UpdateGeneralLocationDTO>().ReverseMap();
        CreateMap<GeneralLocation, CreateGeneralLocationDTO>().ReverseMap();
    }

}
