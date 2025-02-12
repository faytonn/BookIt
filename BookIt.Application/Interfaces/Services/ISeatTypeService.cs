using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Application.Interfaces.Services;

public interface ISeatTypeService : IGetService<GetSeatTypeDTO>, IModifyService<CreateSeatTypeDTO, UpdateSeatTypeDTO>
{
    Task<List<GetSeatTypeDTO>> GetByHall(int hallId);
    Task RestoreAsync(int id);
    Task HardDeleteAsync(int id);
    List<GetSeatTypeDTO> GetArchivedSeatTypes(LanguageType language = LanguageType.English);
}
