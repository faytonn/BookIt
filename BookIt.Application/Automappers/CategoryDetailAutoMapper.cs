using AutoMapper;
using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class CategoryDetailAutoMapper : Profile
{
    public CategoryDetailAutoMapper()
    {
        CreateMap<CategoryDetail, GetCategoryDetailDTO>().ReverseMap();
        CreateMap<CategoryDetail, UpdateCategoryDetailDTO>().ReverseMap();
        CreateMap<CategoryDetail, CreateCategoryDetailDTO>().ReverseMap();
    }
}
