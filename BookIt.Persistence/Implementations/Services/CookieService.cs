using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;

namespace BookIt.Persistence.Services;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILanguageService _languageService;

    public CookieService(IHttpContextAccessor contextAccessor, ILanguageService languageService)
    {
        _contextAccessor = contextAccessor;
        _languageService = languageService;
    }

    public async Task<GetLanguageDTO> GetSelectedLanguageAsync()
    {
        var culture = _contextAccessor.HttpContext?.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
        var isoCode = culture?.Substring(culture.LastIndexOf('=') + 1) ?? "en";
        var selectedLanguage = await _languageService.GetLanguageAsync(x => x.IsoCode == isoCode);

        return selectedLanguage;
    }

    public async Task<LanguageType> GetSelectedLanguageTypeAsync()
    {
        var language = await GetSelectedLanguageAsync();

        var type = LanguageType.English;

        if (language.IsoCode == "en")
            type = LanguageType.English;
        if (language.IsoCode == "aze")
            type = LanguageType.Azerbaijani;
        if (language.IsoCode == "cs")
            type = LanguageType.Czech;

        return type;
    }

    public string GetUserId()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
    }

    public bool IsAuthorized()
    {
        return _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
