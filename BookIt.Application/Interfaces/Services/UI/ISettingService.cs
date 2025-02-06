using BookIt.Application.DTOs.SettingDTO;
using BookIt.Application.Interfaces.Services.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services.UI;

public interface ISettingService : IGetService<GetSettingDTO>
{
    Task<bool> UpdateAsync(UpdateSettingDTO dto, ModelStateDictionary ModelState);
    Task<UpdateSettingDTO> GetUpdatedDtoAsync(int id);
    Task<Dictionary<string, string>> GetSettingDictionaryAsync(int languageId);
}
