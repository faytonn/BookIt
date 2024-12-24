using BookIt.Core.Application.DTOs;

namespace BookIt.Core.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<bool> RequestPasswordResetAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
        Task<bool> VerifyEmailAsync(string token);
        Task<UserDTO> UpdateProfileAsync(int userId, UpdateProfileDTO profileDto);
        Task<UserDTO> UpdateSettingsAsync(int userId, UpdateSettingsDTO settingsDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto);
    }
}
