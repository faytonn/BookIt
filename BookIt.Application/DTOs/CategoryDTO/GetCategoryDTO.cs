using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDTO;

public class GetCategoryDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
}
