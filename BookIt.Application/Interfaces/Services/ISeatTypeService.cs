using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Application.Interfaces.Services.Generic;

namespace BookIt.Application.Interfaces.Services;

public interface ISeatTypeService : IGetService<GetSeatTypeDTO>, IModifyService<CreateSeatTypeDTO, UpdateSeatTypeDTO>
{

}
