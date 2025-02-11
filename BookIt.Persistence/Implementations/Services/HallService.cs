using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.HallDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class HallService : IHallService
{
    private readonly IHallRepository _hallRepository;
    private readonly IMapper _mapper;
    private readonly IGeneralLocationRepository _locationRepository;
    public HallService(IHallRepository hallRepository, IGeneralLocationRepository locationRepository , IMapper mapper)
    {
        _hallRepository = hallRepository;
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<GetHallDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var hall = await _hallRepository.GetAsync(x => x.Id == id, include: h => h.Include(x => x.Location));
        if (hall == null)
            throw new NotFoundException("Hall not found.");

        return _mapper.Map<GetHallDTO>(hall);
    }

    public List<GetHallDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var halls = _hallRepository.GetAll(include: h => h.Include(x => x.Location));
        return _mapper.Map<List<GetHallDTO>>(halls);
    }

    public async Task<PaginateDTO<GetHallDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _hallRepository.GetAll();
        var count = query.Count();
        var pageCount = (int)Math.Ceiling((decimal)count / limit);

        if (page > pageCount)
            page = pageCount;
        if (page < 1)
            page = 1;

        query = _hallRepository.Paginate(query, limit, page);
        var halls = query.ToList();
        var dtos = _mapper.Map<List<GetHallDTO>>(halls);

        return new PaginateDTO<GetHallDTO>
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount,
            //TotalCount = count  // if your PaginateDTO includes TotalCount
        };
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _hallRepository.IsExistAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(CreateHallDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var englishHallName = dto.HallDetails.FirstOrDefault(d => d.LanguageId == 1)?.Name;
        if (string.IsNullOrWhiteSpace(englishHallName))
        {
            modelState.AddModelError("HallDetails", "The hall name in English is required.");
            return false;
        }

        var existing = await _hallRepository.GetAsync(x => x.Name.ToLower() == englishHallName.ToLower());
        if (existing != null)
        {
            modelState.AddModelError("", "A hall with this name already exists.");
            return false;
        }

        var newHall = _mapper.Map<Hall>(dto);
        await _hallRepository.CreateAsync(newHall);
        await _hallRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(UpdateHallDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var locationExists = await _locationRepository.GetAll()
            .AnyAsync(l => l.Id == dto.LocationId && !l.IsDeleted);
        if (!locationExists)
        {
            modelState.AddModelError("LocationId", "The selected location does not exist or is not active.");
            return false;
        }

        var hall = await _hallRepository.GetAsync(x => x.Id == dto.Id);
        if (hall == null)
        {
            modelState.AddModelError("", "Hall not found.");
            return false;
        }

        var englishName = dto.HallDetails.FirstOrDefault(d => d.LanguageId == 1)?.Name;
        if (string.IsNullOrWhiteSpace(englishName))
        {
            modelState.AddModelError("HallDetails", "The hall name in English is required.");
            return false;
        }

        var existing = await _hallRepository.GetAsync(x => x.Name.ToLower() == englishName.ToLower() && x.Id != dto.Id);
        if (existing != null)
        {
            modelState.AddModelError("", "A hall with this name already exists.");
            return false;
        }

        hall = _mapper.Map(dto, hall);
        _hallRepository.Update(hall);
        await _hallRepository.SaveChangesAsync();

        return true;
    }

    public async Task<UpdateHallDTO> GetUpdatedDtoAsync(int id)
    {
        var hall = await _hallRepository.GetAsync(x => x.Id == id);
        if (hall == null)
            throw new NotFoundException("Hall not found.");

        return _mapper.Map<UpdateHallDTO>(hall);
    }

    public async Task DeleteAsync(int id)
    {
        var hall = await _hallRepository.GetAsync(id);
        if (hall == null)
            throw new NotFoundException("Hall not found.");

        _hallRepository.SoftDelete(hall);
        await _hallRepository.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var hall = await _hallRepository.GetAsync(id, ignoreFilter: true);
        if (hall == null)
            throw new NotFoundException("Hall not found.");

        _hallRepository.Repair(hall);
        await _hallRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var hall = await _hallRepository.GetAsync(id, ignoreFilter: true);
        if (hall == null)
            throw new NotFoundException("Hall not found.");

        _hallRepository.HardDelete(hall);

        await _hallRepository.SaveChangesAsync();
    }

    public List<GetHallDTO> GetArchivedHalls(LanguageType language = LanguageType.English)
    {
        var archivedQuery = _hallRepository.GetAll(
                            ignoreFilter: true,
                            include: query => query.Include(h => h.Location))
                        .Where(x => x.IsDeleted);

        var archivedHalls = archivedQuery.ToList();
        return _mapper.Map<List<GetHallDTO>>(archivedHalls);
    }

   
}

