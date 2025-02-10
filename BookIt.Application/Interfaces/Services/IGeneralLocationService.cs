using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.DTOs.GeneralLocationDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface IGeneralLocationService : IGetService<GetGeneralLocationDTO>, IModifyService<CreateGeneralLocationDTO, UpdateGeneralLocationDTO>
{
    public Task RestoreAsync(int id);
    public Task HardDeleteAsync(int id);
    public List<GetGeneralLocationDTO> GetArchivedLocations(LanguageType language = LanguageType.English);
}
