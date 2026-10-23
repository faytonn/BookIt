using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class UserSettingDTO : IDTO
{
    public string Email { get; set; } = string.Empty;
    public UpdateUserProfileDTO UserProfileInfo { get; set; } = new();
    public UpdateUserPasswordDTO UserPasswordInfo { get; set; } = new();
}
