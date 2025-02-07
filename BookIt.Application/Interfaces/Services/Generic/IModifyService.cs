using BookIt.Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services.Generic;

public interface IModifyService<CreateTDTO, UpdateTDTO>
 where CreateTDTO : IDTO
 where UpdateTDTO : IDTO
{
    Task<bool> CreateAsync(CreateTDTO dto, ModelStateDictionary ModelState);
    Task<bool> UpdateAsync(UpdateTDTO dto, ModelStateDictionary ModelState);
    Task<UpdateTDTO> GetUpdatedDtoAsync(int id);
    Task DeleteAsync(int id);
}
