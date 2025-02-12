using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class SeatTypeService : ISeatTypeService
{
    private readonly ISeatTypeRepository _seatTypeRepository;
    private readonly IMapper _mapper;

    public SeatTypeService(ISeatTypeRepository seatTypeRepository, IMapper mapper)
    {
        _seatTypeRepository = seatTypeRepository;
        _mapper = mapper;
    }

    public async Task<List<GetSeatTypeDTO>> GetByHall(int hallId)
    {
        var query = _seatTypeRepository.GetAll(st => st.HallId == hallId && !st.IsDeleted);

        var seatTypes = await query.ToListAsync();
        return _mapper.Map<List<GetSeatTypeDTO>>(seatTypes);
    }

    public async Task<GetSeatTypeDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var seatType = await _seatTypeRepository.GetAsync(x => x.Id == id);
        if (seatType == null)
            throw new NotFoundException("Seat type not found.");
        return _mapper.Map<GetSeatTypeDTO>(seatType);
    }

    public List<GetSeatTypeDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var seatTypes = _seatTypeRepository
                      .GetAll(include: q => q.Include(s => s.Hall))
                      .ToList();
        return _mapper.Map<List<GetSeatTypeDTO>>(seatTypes);
    }

    public async Task<PaginateDTO<GetSeatTypeDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _seatTypeRepository.GetAll();
        var count = await query.CountAsync();
        var pageCount = (int)System.Math.Ceiling((decimal)count / limit);
        if (page > pageCount) page = pageCount;
        if (page < 1) page = 1;
        query = _seatTypeRepository.Paginate(query, limit, page);
        var seatTypes = await query.ToListAsync();
        var dtos = _mapper.Map<List<GetSeatTypeDTO>>(seatTypes);
        return new PaginateDTO<GetSeatTypeDTO>
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount,
        };
    }

    public async Task<bool> CreateAsync(CreateSeatTypeDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var existing = await _seatTypeRepository.GetAsync(x => x.Name.ToLower() == dto.Name.ToLower());
        if (existing != null)
        {
            modelState.AddModelError("", "Seat type already exists.");
            return false;
        }

        var seatType = _mapper.Map<SeatType>(dto);
        await _seatTypeRepository.CreateAsync(seatType);
        await _seatTypeRepository.SaveChangesAsync();
        return true;
    }

    public async Task<UpdateSeatTypeDTO> GetUpdatedDtoAsync(int id)
    {
        var seatType = await _seatTypeRepository.GetAsync(x => x.Id == id);
        if (seatType == null)
            throw new NotFoundException("Seat type not found.");
        return _mapper.Map<UpdateSeatTypeDTO>(seatType);
    }

    public async Task<bool> UpdateAsync(UpdateSeatTypeDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;


        var seatType = await _seatTypeRepository.GetAsync(x => x.Id == dto.Id);

        if (seatType == null)
        {
            modelState.AddModelError("", "Seat type not found.");
            return false;
        }

        var existing = await _seatTypeRepository.GetAsync(
                x => x.Name.ToLower() == dto.Name.ToLower()
                && x.HallId == dto.HallId && x.Id != dto.Id);
        if (existing != null)
        {
            modelState.AddModelError("", "Another seat type with this name already exists.");
            return false;
        }

        seatType = _mapper.Map(dto, seatType);
        _seatTypeRepository.Update(seatType);
        await _seatTypeRepository.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var seatType = await _seatTypeRepository.GetAsync(id);
        if (seatType == null)
            throw new NotFoundException("Seat type not found.");
        _seatTypeRepository.SoftDelete(seatType);
        await _seatTypeRepository.SaveChangesAsync();
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _seatTypeRepository.IsExistAsync(x => x.Id == id);
    }

    public async Task RestoreAsync(int id)
    {
        var seatType = await _seatTypeRepository.GetAsync(id, ignoreFilter: true);
        if (seatType == null)
            throw new NotFoundException("Hall not found.");

        _seatTypeRepository.Repair(seatType);
        await _seatTypeRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var seatType = await _seatTypeRepository.GetAsync(x => x.Id == id, ignoreFilter: true);
        if (seatType == null)
            throw new NotFoundException("Seat type not found.");
        _seatTypeRepository.HardDelete(seatType);
        await _seatTypeRepository.SaveChangesAsync();
    }

    public List<GetSeatTypeDTO> GetArchivedSeatTypes(LanguageType language = LanguageType.English)
    {
        var archived = _seatTypeRepository
                            .GetAll(ignoreFilter: true)
                            .Where(x => x.IsDeleted)
                            .ToList();
        return _mapper.Map<List<GetSeatTypeDTO>>(archived);
    }
}
