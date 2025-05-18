using BookIt.Application.DTOs.AuthorizationDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        IUserAuthorizationService userAuthorizationService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userAuthorizationService = userAuthorizationService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDTO model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            if (await _userAuthorizationService.LoginAsync(model, ModelState))
            {
                var redirectUrl = await _userAuthorizationService.GetRedirectUrlAsync(model.Email!);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return Redirect(redirectUrl);
            }
        }
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var model = _userAuthorizationService.GetRegisterDto(new RegisterDTO());
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDTO model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            if (await _userAuthorizationService.RegisterAsync(model, ModelState))
            {
                return View("ConfirmEmail");
            }
        }
        model = _userAuthorizationService.GetRegisterDto(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _userAuthorizationService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var result = await _userAuthorizationService.EmailVerificationAsync(token, email);
        if (!result)
        {
            return View("EmailVerificationFailure");
        }
        return View("EmailVerificationSuccess");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
    {
        if (ModelState.IsValid)
        {
            if (await _userAuthorizationService.ResetPasswordConfirmationAsync(model, ModelState))
            {
                return View("ResetPasswordConfirmation");
            }
        }
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return BadRequest();
        }

        var model = new ResetPasswordDTO
        {
            Token = token,
            Email = email
        };
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
    {
        if (ModelState.IsValid)
        {
            if (await _userAuthorizationService.ResetPasswordAsync(model, ModelState))
            {
                return View("ResetPasswordSuccess");
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Settings()
    {
        return View();
    }
} 