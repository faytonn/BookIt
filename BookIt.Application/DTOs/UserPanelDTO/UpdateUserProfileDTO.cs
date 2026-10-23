using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class UpdateUserProfileDTO : IDTO
{
    [Required]
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string? LastName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    public LanguageType SelectedNotificationLanguage { get; set; }
}
