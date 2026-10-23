using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.CategoryDTO;

public class UpdateCategoryDTO : IDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public int? CategoryId { get; set; }

    public List<UpdateCategoryDetailDTO> CategoryDetails { get; set; } = [];
}
