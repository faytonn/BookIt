using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BookIt.Persistence.Implementations.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IEventSeatService _eventSeatService;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IEventSeatService eventSeatService, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _eventSeatService = eventSeatService;
        _mapper = mapper;
    }

    public async Task<GetEventDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var ev = await _eventRepository.GetAsync(x => x.Id == id,
                    include: _withRelations());
        if (ev == null)
            throw new NotFoundException("Event not found.");
        return _mapper.Map<GetEventDTO>(ev);
    }

    public async Task<GetEventDTO?> GetAsyncByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return null;

        var ev = await _eventRepository.GetAsync(
            x => x.Title.Trim().ToLower() == title.Trim().ToLower(),
            include: _withRelations()
        );

        if (ev == null)
            return null;

        return _mapper.Map<GetEventDTO>(ev);
    }


    public List<GetEventDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var query = _eventRepository.GetAll(include: _withRelations());
        return _mapper.Map<List<GetEventDTO>>(query.ToList());
    }

    public async Task<PaginateDTO<GetEventDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _eventRepository.GetAll(include: _withRelations());
        var count = await query.CountAsync();
        var pageCount = (int)Math.Ceiling((decimal)count / limit);
        if (page > pageCount) page = pageCount;
        if (page < 1) page = 1;
        query = _eventRepository.Paginate(query, limit, page);
        var events = await query.ToListAsync();
        var dtos = _mapper.Map<List<GetEventDTO>>(events);
        return new PaginateDTO<GetEventDTO>
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount,
        };
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _eventRepository.IsExistAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(CreateEventDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;


        var newEvent = _mapper.Map<Event>(dto);
        await _eventRepository.CreateAsync(newEvent);
        await _eventRepository.SaveChangesAsync();

        // every seat in the hall becomes sellable for this event
        await _eventSeatService.EnsureSeatsForEventAsync(newEvent.Id);
        return true;
    }

    public async Task<UpdateEventDTO> GetUpdatedDtoAsync(int id)
    {
        var ev = await _eventRepository.GetAsync(x => x.Id == id,
                    include: _withRelations());
        if (ev == null)
            throw new NotFoundException("Event not found.");
        return _mapper.Map<UpdateEventDTO>(ev);
    }

    public async Task<bool> UpdateAsync(UpdateEventDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var ev = await _eventRepository.GetAsync(x => x.Id == dto.Id);
        if (ev == null)
        {
            modelState.AddModelError("", "Event not found.");
            return false;
        }

        var hallChanged = ev.HallId != dto.HallId;

        ev = _mapper.Map(dto, ev);
        _eventRepository.Update(ev);
        await _eventRepository.SaveChangesAsync();

        if (hallChanged)
            await _eventSeatService.EnsureSeatsForEventAsync(ev.Id);
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var ev = await _eventRepository.GetAsync(id);
        if (ev == null)
            throw new NotFoundException("Event not found.");
        
        _eventRepository.SoftDelete(ev);

        await _eventRepository.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var ev = await _eventRepository.GetAsync(id, ignoreFilter: true);
        if (ev == null)
            throw new NotFoundException("Event not found.");
        ev.IsDeleted = false;
        await _eventRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var ev = await _eventRepository.GetAsync(id, ignoreFilter: true);
        if (ev == null)
            throw new NotFoundException("Event not found.");
        _eventRepository.HardDelete(ev);
        await _eventRepository.SaveChangesAsync();
    }

    public List<GetEventDTO> GetArchivedEvents(LanguageType language = LanguageType.English)
    {
        var archived = _eventRepository.GetAll(ignoreFilter: true)
                        .Where(x => x.IsDeleted)
                        .ToList();
        return _mapper.Map<List<GetEventDTO>>(archived);
    }

    public async Task<PaginateDTO<GetEventDTO>> SearchAsync(EventFilterDTO filter, LanguageType language = LanguageType.English, int limit = 12)
    {
        var now = DateTime.UtcNow;
        var query = _eventRepository.GetAll(e => e.EventDate >= now, include: _withRelations());

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.Trim();
            query = query.Where(e => e.Title.Contains(term));
        }
        if (filter.LocationId is > 0)
            query = query.Where(e => e.GeneralLocationId == filter.LocationId);
        if (filter.CategoryId is > 0)
            query = query.Where(e => e.CategoryId == filter.CategoryId);
        if (filter.Date.HasValue)
        {
            var day = filter.Date.Value.Date;
            query = query.Where(e => e.EventDate >= day && e.EventDate < day.AddDays(1));
        }

        var count = await query.CountAsync();
        var pageCount = (int)Math.Ceiling((decimal)count / limit);
        var page = filter.Page;
        if (page > pageCount) page = pageCount;
        if (page < 1) page = 1;

        var events = await _eventRepository
            .Paginate(query.OrderBy(e => e.EventDate), limit, page)
            .ToListAsync();

        return new PaginateDTO<GetEventDTO>
        {
            Items = _mapper.Map<List<GetEventDTO>>(events),
            TotalCount = count,
            CurrentPage = page,
            PageSize = limit,
            PageCount = pageCount
        };
    }

    private static Func<IQueryable<Event>, IIncludableQueryable<Event, object>> _withRelations()
    {
        return q => q.Include(e => e.GeneralLocation)
                     .Include(e => e.Category)
                     .Include(e => e.Hall);
    }

    public async Task<List<GetEventDTO>> GetUpcomingEventsAsync(LanguageType language = LanguageType.English)
    {
        var currentDate = DateTime.UtcNow;
        var upcomingEvents = await _eventRepository.GetAll(
            include: _withRelations())
            .Where(e => e.EventDate > currentDate && !e.IsDeleted)
            .OrderBy(e => e.EventDate)
            .Take(6)
            .ToListAsync();

        return _mapper.Map<List<GetEventDTO>>(upcomingEvents);
    }
}
