using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.UI;

namespace BookIt.Persistence.Implementations.Services.UI;

public class LayoutService : ILayoutService
{
    private readonly ICookieService _cookieService;
    private readonly ISettingService _settingService;

    public LayoutService(ICookieService cookieService, ISettingService settingService)
    {
        _cookieService = cookieService;
        _settingService = settingService;
    }

    public async Task<GetLanguageDTO> GetSelectedLanguageAsync()
    {
        return await _cookieService.GetSelectedLanguageAsync();
    }

    public async Task<Dictionary<string, string>> GetSettingsDictionary()
    {
        var language = await GetSelectedLanguageAsync();

        return await _settingService.GetSettingDictionaryAsync(language.Id);
    }
}
