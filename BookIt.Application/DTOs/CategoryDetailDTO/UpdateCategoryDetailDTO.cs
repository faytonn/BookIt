using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDetailDTO;

public class UpdateCategoryDetailDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int LanguageId { get; set; }
    public int? ParentCategoryId { get; set; }  

}