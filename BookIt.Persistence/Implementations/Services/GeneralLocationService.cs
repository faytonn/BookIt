using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.GeneralLocationDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Persistence.Implementations.Services;

public class GeneralLocationService : IGeneralLocationService
{
    private readonly IGeneralLocationRepository _generalLocationRepository;
    private readonly IMapper _mapper;

    public GeneralLocationService(IGeneralLocationRepository generalLocationRepository, IMapper mapper)
    {
        _generalLocationRepository = generalLocationRepository;
        _mapper = mapper;
    }


    public async Task<GetGeneralLocationDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var location = await _generalLocationRepository.GetAsync(x => x.Id == id);
        if (location == null)
            throw new NotFoundException("General location not found.");

        var dto = _mapper.Map<GetGeneralLocationDTO>(location);
        return dto;
    }

    public List<GetGeneralLocationDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var locations = _generalLocationRepository.GetAll();
        var dtos = _mapper.Map<List<GetGeneralLocationDTO>>(locations);
        return dtos;
    }

    public async Task<PaginateDTO<GetGeneralLocationDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _generalLocationRepository.GetAll();
        var count = query.Count();
        var pageCount = (int)Math.Ceiling((decimal)count / limit);

        if (page > pageCount)
            page = pageCount;
        if (page < 1)
            page = 1;

        query = _generalLocationRepository.Paginate(query, limit, page);
        var locations = query.ToList();
        var dtos = _mapper.Map<List<GetGeneralLocationDTO>>(locations);

        return new PaginateDTO<GetGeneralLocationDTO>
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount,
        };
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _generalLocationRepository.IsExistAsync(x => x.Id == id);
    }

 

    public async Task<bool> CreateAsync(CreateGeneralLocationDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var existing = await _generalLocationRepository.GetAsync(x => x.Name.ToLower() == dto.Name.ToLower());
        if (existing != null)
        {
            modelState.AddModelError("", "A general location with this name already exists.");
            return false;
        }

        var newLocation = _mapper.Map<GeneralLocation>(dto);
        await _generalLocationRepository.CreateAsync(newLocation);
        await _generalLocationRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(UpdateGeneralLocationDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var location = await _generalLocationRepository.GetAsync(x => x.Id == dto.Id);
        if (location == null)
        {
            modelState.AddModelError("", "General location not found.");
            return false;
        }

        var existing = await _generalLocationRepository.GetAsync(
            x => x.Name.ToLower() == dto.Name.ToLower() && x.Id != dto.Id);
        if (existing != null)
        {
            modelState.AddModelError("", "A general location with this name already exists.");
            return false;
        }

        location = _mapper.Map(dto, location);
        _generalLocationRepository.Update(location);
        await _generalLocationRepository.SaveChangesAsync();

        return true;
    }

    public async Task<UpdateGeneralLocationDTO> GetUpdatedDtoAsync(int id)
    {
        var location = await _generalLocationRepository.GetAsync(x => x.Id == id);
        if (location == null)
            throw new NotFoundException("General location not found.");

        var dto = _mapper.Map<UpdateGeneralLocationDTO>(location);
        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        var location = await _generalLocationRepository.GetAsync(id);
        if (location == null)
            throw new NotFoundException("General location not found.");
         _generalLocationRepository.SoftDelete(location);
        //Thread.Sleep(100);

        await _generalLocationRepository.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var location = await _generalLocationRepository.GetAsync(id, ignoreFilter: true);
        if (location == null)
            throw new NotFoundException("General location not found.");

        _generalLocationRepository.Repair(location);
        await _generalLocationRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var location = await _generalLocationRepository.GetAsync(id, ignoreFilter: true);
        if (location == null)
            throw new NotFoundException("General location not found.");

        _generalLocationRepository.HardDelete(location);
        await _generalLocationRepository.SaveChangesAsync();
    }

    public List<GetGeneralLocationDTO> GetArchivedLocations(LanguageType language = LanguageType.English)
    {
        var archivedQuery = _generalLocationRepository.GetAll(ignoreFilter: true)
                            .Where(x => x.IsDeleted);
        var archivedLocations = archivedQuery.ToList();
        return _mapper.Map<List<GetGeneralLocationDTO>>(archivedLocations);
    }

}
