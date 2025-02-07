using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.UserPositionDetailDTO;

namespace BookIt.Application.DTOs.UserPositionDTO;

public class GetUserPositionDTO : IDTO
{
    public int Id { get; set; }
    public List<GetUserPositionDetailDTO> UserPositionDetails { get; set; } = [];
}
