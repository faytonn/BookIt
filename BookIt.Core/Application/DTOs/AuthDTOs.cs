using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.Application.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public required string Role { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class ResetPasswordRequestDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        [Required]
        public required string Token { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string NewPassword { get; set; }
    }

    public class AuthResponseDTO
    {
        public required string Token { get; set; }
        public required UserDTO User { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool MarketingEmails { get; set; }
        public bool TwoFactorAuth { get; set; }
    }

    public class UpdateProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
    }

    public class UpdateSettingsDTO
    {
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool MarketingEmails { get; set; }
        public bool TwoFactorAuth { get; set; }
    }

    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
