using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface IEventDetailService : IGetService<GetEventDetailDTO>, IModifyService<CreateEventDetailDTO, UpdateEventDetailDTO>
{
    List<GetEventDetailDTO> GetAllByEventId(int eventId);

    Task<GetEventDetailDTO> GetByEventAndLanguageAsync(int eventId, int languageId);

    Task RestoreAsync(int id);

    Task HardDeleteAsync(int id);

    List<GetEventDetailDTO> GetArchivedEventDetails(int eventId, LanguageType language = LanguageType.English);
}
