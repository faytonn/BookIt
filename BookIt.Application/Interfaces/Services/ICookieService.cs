using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface ICookieService
{
    Task<GetLanguageDTO> GetSelectedLanguageAsync();
    Task<LanguageType> GetSelectedLanguageTypeAsync();
    bool IsAuthorized();
    string GetUserId();
}
