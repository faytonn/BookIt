using BookIt.Application.DTOs.NewsDTO;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface INewsService
{
    Task<List<GetNewsDTO>> GetLatestAsync(LanguageType language = LanguageType.English, int count = 3);
}
