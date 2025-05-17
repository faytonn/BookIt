using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class EventDetailService : IEventDetailService
{
    private readonly IEventDetailRepository _eventDetailRepository;
    private readonly IMapper _mapper;

    public EventDetailService(IEventDetailRepository eventDetailRepository, IMapper mapper)
    {
        _eventDetailRepository = eventDetailRepository;
        _mapper = mapper;
    }

    public async Task<GetEventDetailDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var detail = await _eventDetailRepository.GetAsync(x => x.Id == id);
        if (detail == null)
            throw new NotFoundException("Event detail not found.");
        return _mapper.Map<GetEventDetailDTO>(detail);
    }

    public async Task<List<GetEventDetailDTO>> GetAllAsync(int eventId)
    {
        var details = await _eventDetailRepository.GetAll(x => x.EventId == eventId).ToListAsync();
        return _mapper.Map<List<GetEventDetailDTO>>(details);
    }

    public async Task<PaginateDTO<GetEventDetailDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _eventDetailRepository.GetAll();
        var count = await query.CountAsync();
        var pageCount = (int)Math.Ceiling((decimal)count / limit);
        if (page > pageCount) page = pageCount;
        if (page < 1) page = 1;
        query = _eventDetailRepository.Paginate(query, limit, page);
        var details = await query.ToListAsync();
        var dtos = _mapper.Map<List<GetEventDetailDTO>>(details);
        return new PaginateDTO<GetEventDetailDTO>
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount,
        };
    }

    public async Task<bool> CreateAsync(CreateEventDetailDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        // Check if an event detail for this event and language already exists.
        var existing = await _eventDetailRepository.GetAsync(x => x.EventId == dto.EventId && x.LanguageId == dto.LanguageId);
        if (existing != null)
        {
            modelState.AddModelError("", "Event detail for this language already exists.");
            return false;
        }

        var detail = _mapper.Map<EventDetail>(dto);
        await _eventDetailRepository.CreateAsync(detail);
        await _eventDetailRepository.SaveChangesAsync();
        return true;
    }

    public async Task<UpdateEventDetailDTO> GetUpdatedDtoAsync(int id)
    {
        var detail = await _eventDetailRepository.GetAsync(x => x.Id == id);
        if (detail == null)
            throw new NotFoundException("Event detail not found.");
        return _mapper.Map<UpdateEventDetailDTO>(detail);
    }

    public async Task<bool> UpdateAsync(UpdateEventDetailDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var detail = await _eventDetailRepository.GetAsync(x => x.Id == dto.Id);
        if (detail == null)
        {
            modelState.AddModelError("", "Event detail not found.");
            return false;
        }

        detail = _mapper.Map(dto, detail);
        _eventDetailRepository.Update(detail);
        await _eventDetailRepository.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var detail = await _eventDetailRepository.GetAsync(id);
        if (detail == null)
            throw new NotFoundException("Event detail not found.");

        _eventDetailRepository.SoftDelete(detail);
        
        await _eventDetailRepository.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var detail = await _eventDetailRepository.GetAsync(id, ignoreFilter: true);
        if (detail == null)
            throw new NotFoundException("Event detail not found.");
        _eventDetailRepository.Repair(detail);
        await _eventDetailRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var detail = await _eventDetailRepository.GetAsync(id, ignoreFilter: true);
        if (detail == null)
            throw new NotFoundException("Event detail not found.");
        _eventDetailRepository.HardDelete(detail);
        await _eventDetailRepository.SaveChangesAsync();
    }

    public List<GetEventDetailDTO> GetArchivedEventDetails(int eventId)
    {
        var archived = _eventDetailRepository.GetAll(ignoreFilter: true)
                       .Where(x => x.IsDeleted && x.EventId == eventId)
                       .ToList();
        return _mapper.Map<List<GetEventDetailDTO>>(archived);
    }

    public async Task<GetEventDetailDTO> GetByEventAndLanguageAsync(int eventId, int languageId)
    {
        var detail = await _eventDetailRepository.GetAsync(x => x.EventId == eventId && x.LanguageId == languageId);
        if (detail == null)
            throw new NotFoundException("Event detail not found for the specified language.");
        return _mapper.Map<GetEventDetailDTO>(detail);
    }

    public List<GetEventDetailDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var details = _eventDetailRepository.GetAll(ignoreFilter: false)
                            .Where(x => x.LanguageId == (int)language)
                            .ToList();
        return _mapper.Map<List<GetEventDetailDTO>>(details);
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _eventDetailRepository.IsExistAsync(x => x.Id == id);
    }

    public async Task<List<GetEventDetailDTO>> GetAllByEventId(int eventId)
    {
        var details = await _eventDetailRepository.GetAll(x => x.EventId == eventId).ToListAsync();
        return _mapper.Map<List<GetEventDetailDTO>>(details);
    }

    public List<GetEventDetailDTO> GetAllArchivedEventDetails()
    {
        var archived = _eventDetailRepository.GetAll(ignoreFilter: true)
                       .Where(x => x.IsDeleted)
                       .ToList();
        return _mapper.Map<List<GetEventDetailDTO>>(archived);
    }
}
