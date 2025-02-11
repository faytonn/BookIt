using BookIt.Application.DTOs.EventSeatTypeDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services
{
    public interface IEventDetailSeatTypeService : IGetService<GetEventDetailSeatTypeDTO>, IModifyService<CreateEventDetailSeatTypeDTO, UpdateEventDetailSeatTypeDTO>
    {
        Task RestoreAsync(int id);
        Task HardDeleteAsync(int id);
        List<GetEventDetailSeatTypeDTO> GetArchivedEventSeatTypes(int eventDetailId, LanguageType language = LanguageType.English);
    }
}
