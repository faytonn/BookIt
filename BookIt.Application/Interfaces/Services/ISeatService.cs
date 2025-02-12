using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services;

public interface ISeatService : IGetService<GetSeatDTO>, IModifyService<CreateSeatDTO, UpdateSeatDTO>
{
    //Task CreateAsync(CreateSeatDTO dto);
    Task<bool> BulkCreateSeatsAsync(CreateBulkSeatDTO dto, ModelStateDictionary modelState);
    //Task UpdateSeatAsync(UpdateSeatDTO dto);
    Task<IEnumerable<GetSeatDTO>> GetSeatsByHallAsync(int hallId);
    Task RestoreAsync(int id);
    Task HardDeleteAsync(int id);
    List<GetSeatDTO> GetArchivedSeats(LanguageType language = LanguageType.English);
}
