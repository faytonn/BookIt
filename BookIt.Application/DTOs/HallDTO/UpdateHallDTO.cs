using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.HallDTO;

public class UpdateHallDTO : IDTO
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    //public string Name { get; set; } = null!;
    public List<HallDetailDTO> HallDetails { get; set; } = [];
}