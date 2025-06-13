using BookIt.Application.DTOs.AuthorizationDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Application.DTOs.EmailDTO;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Infrastracture.Implementations.Services;

public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserAuthorizationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _contextAccessor = contextAccessor;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext ?? new());
    }

    public async Task<bool> RegisterAsync(RegisterDTO dto, ModelStateDictionary ModelState)
    {
        if (!ModelState.IsValid)
            return false;

        if (dto.Password != dto.ConfirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return false;
        }

        if (await IsEmailUniqueAsync(dto.Email))
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true,
                SelectedLanguage = dto.NotificationLanguage
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                // Add user to role based on RegistrationRole
                await _userManager.AddToRoleAsync(user, dto.RegistrationRole.ToString());
                
                // Send email verification
                await _sendConfirmEmailTokenAsync(user);
                return true;
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Email is already taken.");
        }

        return false;
    }

    private async Task<bool> IsEmailUniqueAsync(string? email)
    {
        return !await _userManager.Users.AnyAsync(u => u.Email == email);
    }

    private async Task _sendConfirmEmailTokenAsync(ApplicationUser user)
    {
        string confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var context = new UrlActionContext
        {
            Action = "VerifyEmail",
            Controller = "Account",
            Values = new { token = confirmEmailToken, email = user.Email },
            Protocol = _contextAccessor.HttpContext?.Request.Scheme
        };

        var link = _urlHelper.Action(context);

        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StaticFiles", "ConfirmEmailBody.html");
        var emailBody = await File.ReadAllTextAsync(templatePath);
        emailBody = emailBody
            .Replace("{First Name}", user.FirstName)
            .Replace("{Verification Link}", link);

        var emailSendDto = new SendEmailDTO
        {
            Body = emailBody,
            Subject = "Email Confirmation",
            ToEmail = user.Email!
        };

        await _emailService.SendEmailAsync(emailSendDto);
    }

    public RegisterDTO GetRegisterDto(RegisterDTO dto, LanguageType language = LanguageType.Azerbaijani)
    {
        // Implement localization logic here if needed
        return dto;
    }

    public async Task<bool> LoginAsync(LoginDTO dto, ModelStateDictionary ModelState)
    {
        if (!ModelState.IsValid)
            return false;

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return false;
        }

        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError(string.Empty, "Please confirm your email first.");
            await _sendConfirmEmailTokenAsync(user);
            return false;
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Your account has been deactivated.");
            return false;
        }

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return false;
        }

        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            return true;
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return false;
    }

    public async Task<bool> ResetPasswordConfirmationAsync(ForgotPasswordDTO dto, ModelStateDictionary ModelState)
    {
        if (!ModelState.IsValid)
            return false;

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return true;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var context = new UrlActionContext
        {
            Action = "ResetPassword",
            Controller = "Account",
            Values = new { token, email = user.Email },
            Protocol = _contextAccessor.HttpContext?.Request.Scheme
        };

        var link = _urlHelper.Action(context);

        var emailBody = $"Reset your password by clicking <a href='{link}'>here</a>";

        var emailSendDto = new SendEmailDTO
        {
            Body = emailBody,
            Subject = "Password Reset",
            ToEmail = user.Email
        };

        await _emailService.SendEmailAsync(emailSendDto);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto, ModelStateDictionary ModelState)
    {
        if (!ModelState.IsValid)
            return false;

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid attempt.");
            return false;
        }

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
        if (result.Succeeded)
        {
            return true;
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return false;
    }

    public async Task<bool> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return true;
    }

    public async Task<bool> EmailVerificationAsync(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<ApplicationUser> GetAuthenticatedUserAsync()
    {
        return await _userManager.GetUserAsync(_signInManager.Context.User);
    }

    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new List<string>();
        }

        return (await _userManager.GetRolesAsync(user)).ToList();
    }

    public async Task<List<ApplicationUser>> GetAllMembersAsync()
    {
        return await _userManager.Users
            .Where(u => u.IsActive)
            .ToListAsync();
    }

    public async Task<string> GetRedirectUrlAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return "/";
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
        {
            return "/Admin/Home/Index";
        }

        return "/";
    }
} 