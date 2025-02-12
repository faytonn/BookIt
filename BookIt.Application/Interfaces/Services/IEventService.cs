using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.DTOs.HallDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface IEventService : IGetService<GetEventDTO>,IModifyService<CreateEventDTO, UpdateEventDTO>
{
    Task<GetEventDTO?> GetAsyncByTitle(string title);
    public Task RestoreAsync(int id);
    public Task HardDeleteAsync(int id);
    public List<GetEventDTO> GetArchivedEvents(LanguageType language = LanguageType.English);
}
