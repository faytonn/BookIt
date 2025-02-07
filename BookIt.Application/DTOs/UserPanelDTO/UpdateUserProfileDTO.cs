using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class UpdateUserProfileDTO : IDTO
{
    public string? UserName { get; set; } 
    public string? FirstName { get; set; }
    public string? LastName { get; set;}
    public string? Email { get; set; }
    public LanguageType SelectedNotificationLanguage { get; set; }
}
