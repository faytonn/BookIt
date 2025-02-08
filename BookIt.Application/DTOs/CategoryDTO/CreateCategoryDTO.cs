using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDTO;

public class CreateCategoryDTO : IDTO
{
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }

    public List<CreateCategoryDetailDTO> CategoryDetails { get; set; } = [];
}
