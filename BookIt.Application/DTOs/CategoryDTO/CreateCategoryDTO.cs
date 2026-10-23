using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.CategoryDTO;

public class CreateCategoryDTO : IDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }

    public List<CreateCategoryDetailDTO> CategoryDetails { get; set; } = [];
}
