using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;
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
        // The request localization middleware has already resolved the cookie / default culture.
        var isoCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var selectedLanguage = await _languageService.GetLanguageAsync(x => x.IsoCode == isoCode)
                               ?? await _languageService.GetLanguageAsync(x => x.IsoCode == "en");

        return selectedLanguage;
    }

    public async Task<LanguageType> GetSelectedLanguageTypeAsync()
    {
        var language = await GetSelectedLanguageAsync();

        return language.IsoCode switch
        {
            "az" => LanguageType.Azerbaijani,
            "cs" => LanguageType.Czech,
            _ => LanguageType.English
        };
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
