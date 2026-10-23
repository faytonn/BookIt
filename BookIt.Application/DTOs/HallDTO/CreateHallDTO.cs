using BookIt.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.HallDTO;

public class CreateHallDTO : IDTO
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Please select a location.")]
    public int LocationId { get; set; }
}
