using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.AuthorizationDTO;

public class ResetPasswordDTO
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }

    [Required]
    public string? Token { get; set; }
}
