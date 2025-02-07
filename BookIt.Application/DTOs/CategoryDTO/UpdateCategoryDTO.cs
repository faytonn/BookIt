using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDTO;

public class UpdateCategoryDTO : IDTO
{
    public string Name { get; set; } = null!;
    public int? CategoryId { get; set; }
}
