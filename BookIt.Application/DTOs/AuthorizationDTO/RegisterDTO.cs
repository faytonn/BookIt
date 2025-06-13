using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.AuthorizationDTO;

public class RegisterDTO
{
    public string? FirstName { get; set; }

    public string? LastName { get; set;}

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }
    public bool IsActive { get; set; } = true;
    public RegistrationRole RegistrationRole { get; set; }
    public LanguageType NotificationLanguage { get; set; }
    public CountryCodeType CountryCode { get; set; }


}
