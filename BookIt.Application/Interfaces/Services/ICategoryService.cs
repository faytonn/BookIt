using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.Interfaces.Services.Generic;

namespace BookIt.Application.Interfaces.Services;

public interface ICategoryService : IGetService<GetCategoryDTO>, IModifyService<CreateCategoryDTO, UpdateCategoryDTO>
{
    Task<bool> AddCategoryDetailAsync(CreateCategoryDetailDTO categoryDetailDTO);
    Task<bool> UpdateCategoryDetailAsync(UpdateCategoryDetailDTO categoryDetailDTO);
    Task DeleteCategoryDetailAsync(int categoryDetailId);
}
