using BookIt.Application.DTOs.AuthorizationDTO;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services;

public interface IUserAuthorizationService
{
    Task<bool> RegisterAsync(RegisterDTO dto, ModelStateDictionary ModelState);
    RegisterDTO GetRegisterDto(RegisterDTO dto, LanguageType language = LanguageType.Azerbaijani);
    Task<bool> LoginAsync(LoginDTO dto, ModelStateDictionary ModelState);
    Task<bool> ResetPasswordConfirmationAsync(ForgotPasswordDTO dto, ModelStateDictionary ModelState);
    Task<bool> ResetPasswordAsync(ResetPasswordDTO dto, ModelStateDictionary ModelState);
    Task<bool> LogoutAsync();
    Task<bool> EmailVerificationAsync(string token, string email);
    Task<ApplicationUser> GetAuthenticatedUserAsync();
    Task<List<string>> GetUserRolesAsync(string userId);
    Task<List<ApplicationUser>> GetAllMembersAsync();
    Task<string> GetRedirectUrlAsync(string email);
} 