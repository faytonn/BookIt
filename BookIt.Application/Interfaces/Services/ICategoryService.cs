using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface ICategoryService : IGetService<GetCategoryDTO>, IModifyService<CreateCategoryDTO, UpdateCategoryDTO>
{
    public Task RestoreAsync(int id);
    public Task HardDeleteAsync(int id);
    public List<GetCategoryDTO> GetArchivedCategories(LanguageType language = LanguageType.English);
}
