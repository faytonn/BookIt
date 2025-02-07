using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.HallDTO;

public class GetHallDTO : IDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int LocationId { get; set; }
}
