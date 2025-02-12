using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface ISeatTypeService : IGetService<GetSeatTypeDTO>, IModifyService<CreateSeatTypeDTO, UpdateSeatTypeDTO>
{
    Task RestoreAsync(int id);
    Task HardDeleteAsync(int id);
    List<GetSeatTypeDTO> GetArchivedSeatTypes(LanguageType language = LanguageType.English);
}
