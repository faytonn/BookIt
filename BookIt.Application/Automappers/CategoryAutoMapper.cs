using AutoMapper;
using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class CategoryAutoMapper : Profile
{
    public CategoryAutoMapper()
    {
        CreateMap<Category, GetCategoryDTO>().ReverseMap();
        CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
        CreateMap<Category, CreateCategoryDTO>().ReverseMap();

        CreateMap<CreateCategoryDetailDTO, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

        CreateMap<UpdateCategoryDetailDTO, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));


    }
}
