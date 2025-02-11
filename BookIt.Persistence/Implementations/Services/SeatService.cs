using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    private readonly IMapper _mapper;

    public SeatService(ISeatRepository seatRepository, IMapper mapper)
    {
        _seatRepository = seatRepository;
        _mapper = mapper;
    }

    public async Task<GetSeatDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var seat = await _seatRepository.GetAsync(x => x.Id == id, include: q => q.Include(s => s.SeatType));
        if (seat == null)
            throw new NotFoundException("Seat not found.");
        return _mapper.Map<GetSeatDTO>(seat);
    }

    public List<GetSeatDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var seats = _seatRepository.GetAll(include: q => q.Include(s => s.SeatType)).ToList();
        return _mapper.Map<List<GetSeatDTO>>(seats);
    }

    public async Task<PaginateDTO<GetSeatDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _seatRepository.GetAll(include: q => q.Include(s => s.SeatType));
        var count = await query.CountAsync();
        var pageCount = (int)System.Math.Ceiling((decimal)count / limit);
        if (page > pageCount) page = pageCount;
        if (page < 1) page = 1;
        query = _seatRepository.Paginate(query, limit, page);
        var seats = await query.ToListAsync();
        var dtos = _mapper.Map<List<GetSeatDTO>>(seats);
        return new PaginateDTO<GetSeatDTO>
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount,
        };
    }

    public async Task<bool> CreateAsync(CreateSeatDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        // Optionally check if the seat already exists in that row and column for the hall.
        var existing = await _seatRepository.GetAsync(x => x.SeatName.ToLower() == dto.SeatName.ToLower() && x.HallId == dto.HallId);
        if (existing != null)
        {
            modelState.AddModelError("", "Seat already exists in the hall.");
            return false;
        }

        var seat = _mapper.Map<Seat>(dto);
        await _seatRepository.CreateAsync(seat);
        await _seatRepository.SaveChangesAsync();
        return true;
    }

    public async Task<UpdateSeatDTO> GetUpdatedDtoAsync(int id)
    {
        var seat = await _seatRepository.GetAsync(x => x.Id == id);
        if (seat == null)
            throw new NotFoundException("Seat not found.");
        return _mapper.Map<UpdateSeatDTO>(seat);
    }

    public async Task<bool> UpdateAsync(UpdateSeatDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var seat = await _seatRepository.GetAsync(x => x.Id == dto.Id);
        //seat = await _seatRepository.GetAsync(x => x.Id == dto.HallId);
       
        if (seat == null)
        {
            modelState.AddModelError("", "Seat not found.");
            return false;
        }

        var existing = await _seatRepository.GetAsync(x => x.SeatName.ToLower() == dto.SeatName.ToLower() && x.Id != dto.Id && x.HallId == dto.HallId);
        if (existing != null)
        {
            modelState.AddModelError("", "Another seat with this name already exists in the hall.");
            return false;
        }

        seat = _mapper.Map(dto, seat);
        _seatRepository.Update(seat);
        await _seatRepository.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var seat = await _seatRepository.GetAsync(id);
        if (seat == null)
            throw new NotFoundException("Seat not found.");
        _seatRepository.SoftDelete(seat);
        await _seatRepository.SaveChangesAsync();
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _seatRepository.IsExistAsync(x => x.Id == id);
    }
}
