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

namespace BookIt.Persistence.Implementations.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<GetEventDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var ev = await _eventRepository.GetAsync(x => x.Id == id,
                    include: q => q.Include(e => e.GeneralLocation)
                                    .Include(e => e.Category));
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
            include: q => q.Include(e => e.GeneralLocation)
                          .Include(e => e.Category)
        );

        if (ev == null)
            return null;

        return _mapper.Map<GetEventDTO>(ev);
    }


    public List<GetEventDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var query = _eventRepository.GetAll(include: q => q.Include(e => e.GeneralLocation)
                                                           .Include(e => e.Category));
        return _mapper.Map<List<GetEventDTO>>(query.ToList());
    }

    public async Task<PaginateDTO<GetEventDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _eventRepository.GetAll(include: q => q.Include(e => e.GeneralLocation)
                                                           .Include(e => e.Category));
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
        return true;
    }

    public async Task<UpdateEventDTO> GetUpdatedDtoAsync(int id)
    {
        var ev = await _eventRepository.GetAsync(x => x.Id == id,
                    include: q => q.Include(e => e.GeneralLocation)
                                    .Include(e => e.Category));
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

        ev = _mapper.Map(dto, ev);
        _eventRepository.Update(ev);
        await _eventRepository.SaveChangesAsync();
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
}
