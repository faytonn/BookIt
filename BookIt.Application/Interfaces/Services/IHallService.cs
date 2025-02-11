using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.DTOs.HallDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface IHallService : IGetService<GetHallDTO>, IModifyService<CreateHallDTO, UpdateHallDTO> 
{
    public Task RestoreAsync(int id);
    public Task HardDeleteAsync(int id);
    public List<GetHallDTO> GetArchivedHalls(LanguageType language = LanguageType.English);
}
