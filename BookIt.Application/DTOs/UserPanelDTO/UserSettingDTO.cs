using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class UserSettingDTO : IDTO
{
    public UpdateUserProfileDTO UserProfileInfo { get; set; } = new();
    public UpdateUserPasswordDTO UserPasswordInfo { get; set; } = new();
}
