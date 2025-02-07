using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class GetUserNameDTO : IDTO
{
    public string FullName { get; set; } = null!;
}
