using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDTO;

public class UpdateCategoryDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? CategoryId { get; set; }

    public List<UpdateCategoryDetailDTO> CategoryDetails { get; set; } = [];
}
