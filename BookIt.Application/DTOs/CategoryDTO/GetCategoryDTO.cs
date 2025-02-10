using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CategoryDTO;

public class GetCategoryDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsDeleted {  get; set; }
    public int? ParentCategoryId { get; set; }
    
    public List<GetCategoryDetailDTO> CategoryDetails { get; set; } = [];

}
