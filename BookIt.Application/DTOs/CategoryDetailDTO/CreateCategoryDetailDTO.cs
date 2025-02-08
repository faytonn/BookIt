using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDetailDTO;

public class CreateCategoryDetailDTO : IDTO
{
    public string? Name { get; set; }
    public int LanguageId { get; set; }
    public int? ParentCategoryId { get; set; }
}
