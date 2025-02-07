using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.HallDTO;

public class UpdateHallDTO : IDTO
{
    public string Name { get; set; } = null!;
    public int LocationId { get; set; }
}