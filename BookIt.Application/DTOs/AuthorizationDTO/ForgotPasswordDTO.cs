using System.ComponentModel.DataAnnotations;

namespace BookIt.Application.DTOs.AuthorizationDTO;

public class ForgotPasswordDTO
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
