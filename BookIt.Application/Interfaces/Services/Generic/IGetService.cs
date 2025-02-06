using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services.Generic;

public interface IGetService<GetTDTO> where GetTDTO : IDTO
{
    Task<GetTDTO> GetAsync(int id, LanguageType language = LanguageType.English);
    List<GetTDTO> GetAll(LanguageType language = LanguageType.English);
    Task<PaginateDTO<GetTDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10);
    Task<bool> IsExistAsync(int id);
}
