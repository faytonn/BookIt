using BookIt.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.UserPanelDTO;

public class UpdateUserPasswordDTO : IDTO
{
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}
