using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.Interfaces.Services.Generic;

namespace BookIt.Application.Interfaces.Services;

public interface ISeatService : IGetService<GetSeatDTO>, IModifyService<CreateSeatDTO, UpdateSeatDTO>
{

}
