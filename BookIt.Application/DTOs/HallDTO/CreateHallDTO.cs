using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.HallDTO;

public class CreateHallDTO : IDTO
{
    public int LocationId { get; set; }
    //public string Name { get; set; }
    public List<HallDetailDTO> HallDetails { get; set; } = [];

}
