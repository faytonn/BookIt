using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class UpdateUserPasswordDTO : IDTO
{
    public string? Password { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}
