using BookIt.Application.DTOs.LanguageDTO;

namespace BookIt.Application.Interfaces.Services.UI;

public interface ILayoutService
{
    Task<GetLanguageDTO> GetSelectedLanguageAsync();
    Task<Dictionary<string, string>> GetSettingsDictionary();
}
