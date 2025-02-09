using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDTO;

namespace BookIt.Application.DTOs.CategoryDetailDTO;

public class GetCategoryDetailDTO : IDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int LanguageId { get; set; }
    public int? ParentCategoryId { get; set; }
    public GetCategoryDTO? ParentCategory { get; set; }
    public ICollection<GetCategoryDTO> ChildCategories { get; set; } = [];
    public ICollection<GetEventDTO> Events { get; set; } = [];
}
