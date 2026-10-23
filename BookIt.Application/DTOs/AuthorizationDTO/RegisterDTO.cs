using BookIt.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.AuthorizationDTO;

public class RegisterDTO
{
    [Required]
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }

    public bool IsActive { get; set; } = true;
    public LanguageType NotificationLanguage { get; set; }
    public CountryCodeType CountryCode { get; set; }
}
