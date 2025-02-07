using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.HallDTO;

public class CreateHallDTO : IDTO
{
    public string Name { get; set; } = null!;
    public int LocationId { get; set; }
}
