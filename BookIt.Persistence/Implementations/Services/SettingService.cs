using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.SettingDTO;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.UI;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class SettingService : ISettingService
{
    private readonly ISettingRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILanguageService _languageService;

    public SettingService(ISettingRepository repository, IMapper mapper, ILanguageService languageService)
    {
        _repository = repository;
        _mapper = mapper;
        _languageService = languageService;
    }

    public List<GetSettingDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var settings = _repository.GetAll(include: x => x.Include(x => x.SettingDetails.Where(x => x.LanguageId == (int)language)));

        var dtos = _mapper.Map<List<GetSettingDTO>>(settings);

        return dtos;
    }

    public async Task<GetSettingDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var setting = await _repository.GetAsync(x => x.Id == id, x => x.Include(x => x.SettingDetails.Where(x => x.LanguageId == (int)language)));

        if (setting == null)
            Console.WriteLine("not found");

        var dto = _mapper.Map<GetSettingDTO>(setting);

        return dto;
    }

    public Task<PaginateDTO<GetSettingDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, string>> GetSettingDictionaryAsync(int languageId)
    {
        var settings = _repository.GetAll(include: x => x.Include(x => x.SettingDetails.Where(x => x.LanguageId == languageId)))
                                 .ToDictionaryAsync(x => x.Key, x => x.SettingDetails.FirstOrDefault()?.Value ?? "");

        return settings;
    }

    public async Task<UpdateSettingDTO> GetUpdatedDtoAsync(int id)
    {
        var setting = await _repository.GetAsync(x => x.Id == id, x => x.Include(x => x.SettingDetails));

        if (setting == null)
            Console.WriteLine("not found");

        var dto = _mapper.Map<UpdateSettingDTO>(setting);

        return dto;
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _repository.IsExistAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateAsync(UpdateSettingDTO dto, ModelStateDictionary ModelState)
    {
        if (!ModelState.IsValid)
            return false;

        var setting = await _repository.GetAsync(x => x.Id == dto.Id, x => x.Include(x => x.SettingDetails));

        if (setting == null)
            Console.WriteLine("tapilmadi");

        foreach (var detail in dto.SettingDetails)
        {
            var existLanguage = await _languageService.GetLanguageAsync(x => x.Id == detail.LanguageId);

            if (existLanguage == null)
            {
                ModelState.AddModelError("", "Nə isə yanlış oldu, yenidən sınayın");
                return false;
            }
        }

        setting = _mapper.Map(dto, setting);

        _repository.Update(setting);
        await _repository.SaveChangesAsync();

        return true;

    }
}
