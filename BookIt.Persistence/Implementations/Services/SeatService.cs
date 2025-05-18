using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;
    private readonly IReservationSeatRepository _reservationSeatRepository;

    public SeatService(ISeatRepository seatRepository, IMapper mapper, IEventRepository eventRepository, IReservationSeatRepository reservationSeatRepository)
    {
        _seatRepository = seatRepository;
        _mapper = mapper;
        _eventRepository = eventRepository;
        _reservationSeatRepository = reservationSeatRepository;
    }

    public async Task<bool> CreateAsync(CreateSeatDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var existingSeat = await _seatRepository.GetAsync(
            s => s.HallId == dto.HallId
                 && s.SeatRow == dto.SeatRow
                 && s.SeatColumn == dto.SeatColumn);

        if (existingSeat != null)
        {
            modelState.AddModelError("", "A seat at this row and column already exists in this hall.\n(Maybe check the Archived page?)");
            return false;
        }

        if (string.IsNullOrWhiteSpace(dto.SeatName))
        {
            dto.SeatName = $"{(char)('A' + dto.SeatRow - 1)}{dto.SeatColumn}";
        }

        var seat = _mapper.Map<Seat>(dto);
        seat.IsReserved = false;

        await _seatRepository.CreateAsync(seat);
        await _seatRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> BulkCreateSeatsAsync(CreateBulkSeatDTO dto, ModelStateDictionary modelState)
    {
        if(!modelState.IsValid) 
            return false;

        if (dto.StartRow > dto.EndRow || dto.StartColumn > dto.EndColumn)
            throw new ArgumentException("Invalid row or column range for bulk creation.");

        for (int row = dto.StartRow; row <= dto.EndRow; row++)
        {
            for (int col = dto.StartColumn; col <= dto.EndColumn; col++)
            {
                var existingSeat = await _seatRepository.GetAsync(
                    s => s.HallId == dto.HallId &&
                         s.SeatRow == row &&
                         s.SeatColumn == col);

                if (existingSeat != null)
                {
                    continue;
                }

                string seatName = $"{(char)('A' + row - 1)}{col}";

                var seat = new Seat
                {
                    HallId = dto.HallId,
                    SeatRow = row,
                    SeatColumn = col,
                    SeatName = seatName,
                    SeatTypeId = dto.SeatTypeId,
                    IsReserved = false
                };

               await _seatRepository.CreateAsync(seat);
            }
        }
        await _seatRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateSeatDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var seat = await _seatRepository.GetAsync(s => s.Id == dto.Id);
        if (seat == null)
            throw new NotFoundException("Seat not found.");

        _mapper.Map(dto, seat);
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

    public async Task<IEnumerable<GetSeatDTO>> GetSeatsByHallAsync(int hallId)
    {
        var seats = await _seatRepository
            .GetAll(predicate: s => s.HallId == hallId && !s.IsDeleted)
            .OrderBy(s => s.SeatRow)
            .ThenBy(s => s.SeatColumn)
            .ToListAsync();

        return _mapper.Map<IEnumerable<GetSeatDTO>>(seats);
    }

    public async Task RestoreAsync(int id)
    {
        var seat = await _seatRepository.GetAsync(id, ignoreFilter: true);
        if (seat == null)
            throw new NotFoundException("Seat not found.");

        _seatRepository.Repair(seat);
        await _seatRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var seat = await _seatRepository.GetAsync(id, ignoreFilter: true);
        if (seat == null)
            throw new NotFoundException("Seat not found.");

        _seatRepository.HardDelete(seat);
        await _seatRepository.SaveChangesAsync();
    }

    public List<GetSeatDTO> GetArchivedSeats(LanguageType language = LanguageType.English)
    {
        var archivedSeats = _seatRepository
                                .GetAll(ignoreFilter: true)
                                .Where(s => s.IsDeleted)
                                .OrderBy(s => s.SeatRow)
                                .ThenBy(s => s.SeatColumn)
                                .ToList();
        return _mapper.Map<List<GetSeatDTO>>(archivedSeats);
    }


    public async Task<GetSeatDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var seat = await _seatRepository.GetAsync(
            s => s.Id == id && !s.IsDeleted);
        if (seat == null)
            throw new NotFoundException("Seat not found.");
        return _mapper.Map<GetSeatDTO>(seat);
    }

 
    public List<GetSeatDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var seats = _seatRepository
            .GetAll(predicate: s => !s.IsDeleted)
            .ToList();
        return _mapper.Map<List<GetSeatDTO>>(seats);
    }

 
    public async Task<PaginateDTO<GetSeatDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _seatRepository.GetAll(predicate: s => !s.IsDeleted);
        int count = query.Count();
        int pageCount = (int)Math.Ceiling((decimal)count / limit);

        if (page > pageCount)
            page = pageCount;
        if (page < 1)
            page = 1;

        // Assuming your repository provides a Paginate method:
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

 
    public async Task<bool> IsExistAsync(int id)
    {
        return await _seatRepository.IsExistAsync(s => s.Id == id && !s.IsDeleted);
    }

 
    public async Task<UpdateSeatDTO> GetUpdatedDtoAsync(int id)
    {
        var seat = await _seatRepository.GetAsync(
            s => s.Id == id && !s.IsDeleted);
        if (seat == null)
            throw new NotFoundException("Seat not found.");
        return _mapper.Map<UpdateSeatDTO>(seat);
    }

    public async Task<List<GetSeatDTO>> GetAvailableSeatsForEventAsync(int eventId)
    {
        var ev = await _eventRepository.GetAsync(eventId);
        if (ev == null) throw new NotFoundException("Event not found.");

        var allSeats = await _seatRepository.GetAll(s => s.HallId == ev.HallId && !s.IsDeleted).ToListAsync();

        var reservedSeatIds = (await _reservationSeatRepository.GetActiveByEventIdAsync(eventId))
            .Select(rs => rs.SeatId)
            .ToHashSet();

        var availableSeats = allSeats.Where(s => !reservedSeatIds.Contains(s.Id)).ToList();

        return _mapper.Map<List<GetSeatDTO>>(availableSeats);
    }

    

    
}

