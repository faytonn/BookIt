using AutoMapper;
using BookIt.Application.DTOs.Common;
using BookIt.Application.DTOs.EventAndDetailsCombinedDTO;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Persistence.Implementations;

public class EventCompositeService : IEventCompositeService
{
    private readonly IEventService _eventService;
    private readonly IEventDetailService _eventDetailService;
    private readonly IEventRepository _eventRepository;
    private readonly IEventDetailRepository _eventDetailRepository;
    //private readonly I
    private readonly IMapper _mapper;

    public EventCompositeService(IEventService eventService, IEventDetailService eventDetailService, IMapper mapper, IEventDetailRepository eventDetailRepository, IEventRepository eventRepository)
    {
        _eventService = eventService;
        _eventDetailService = eventDetailService;
        _mapper = mapper;
        _eventDetailRepository = eventDetailRepository;
        _eventRepository = eventRepository;
    }


    public async Task<bool> CreateAsync(CreateEventCompositeDTO dto, ModelStateDictionary ModelState)
    {
        return await CreateEventWithDetailsAsync(dto, ModelState);
    }


    public async Task<bool> UpdateAsync(UpdateEventCompositeDTO dto, ModelStateDictionary modelState)
    {
        return await UpdateEventWithDetailsAsync(dto, modelState);
    }


    public async Task<UpdateEventCompositeDTO> GetUpdatedDtoAsync(int id)
    {
        var composite = await GetAsync(id);
        return _mapper.Map<UpdateEventCompositeDTO>(composite);
    }

    public async Task<GetEventCompositeDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var ev = await _eventService.GetAsync(id, language);
        if (ev == null)
            throw new NotFoundException("Event not found.");

        var details = _eventDetailService.GetAll(LanguageType.English);
        return new GetEventCompositeDTO
        {
            Event = ev,
            EventDetails = details
        };
    }


    public List<GetEventCompositeDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var events = _eventService.GetAll(language);
        var list = new List<GetEventCompositeDTO>();

        foreach (var ev in events)
        {
            var details = _eventDetailService.GetAll();
            list.Add(new GetEventCompositeDTO
            {
                Event = ev,
                EventDetails = details
            });
        }
        return list;
    }


    public async Task<PaginateDTO<GetEventCompositeDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        //var eventPages = await _eventService.GetPagesAsync(language, page, limit);
        //var compositeList = new List<GetEventCompositeDTO>();

        //foreach (var ev in eventPages.Items)
        //{
        //    var details = _eventDetailService.GetAll();
        //    compositeList.Add(new GetEventCompositeDTO
        //    {
        //        Event = ev,
        //        EventDetails = details
        //    });
        //}

        //return new PaginateDTO<GetEventCompositeDTO>
        //{
        //    Items = compositeList,
        //    CurrentPage = eventPages.CurrentPage,
        //    PageCount = eventPages.PageCount
        //};

        throw new NotImplementedException();
    }




    public async Task DeleteAsync(int id)
    {
        var exists = await _eventRepository.GetAsync(id);
        if (exists == null)
            throw new NotFoundException("Event not found.");

        _eventRepository.SoftDelete(exists);

        var details = _eventDetailRepository.GetAll(d => d.EventId == exists.Id).ToList();
        foreach (var detail in details)
        {
            _eventDetailRepository.SoftDelete(detail);
        }

        await _eventRepository.SaveChangesAsync();
        await _eventDetailRepository.SaveChangesAsync();
    }


    public async Task HardDeleteAsync(int id, ModelStateDictionary modelState)
    {
        var ev = await _eventRepository.GetAsync(id, ignoreFilter: true);
        if (ev == null)
            throw new NotFoundException("Event not found.");
        _eventRepository.HardDelete(ev);
        await _eventRepository.SaveChangesAsync();
    }


    public async Task RestoreAsync(int id, ModelStateDictionary modelState)
    {
        var ev = await _eventRepository.GetAsync(id, ignoreFilter: true);
        if (ev == null)
            throw new NotFoundException("Event not found.");
        ev.IsDeleted = false;
        await _eventRepository.SaveChangesAsync();
    }

    public Task<bool> IsExistAsync(int id)
    {
        return _eventService.IsExistAsync(id);
    }

    public async Task<List<GetEventCompositeDTO>> GetArchivedEvents(LanguageType language = LanguageType.English)
    {
        var archivedEvents = _eventService.GetArchivedEvents(language);
        var list = new List<GetEventCompositeDTO>();

        foreach (var ev in archivedEvents)
        {
            var details = await _eventDetailService.GetAllByEventId(ev.Id);
            list.Add(new GetEventCompositeDTO
            {
                Event = ev,
                EventDetails = details
            });
        }
        return list;
    }


    private async Task<bool> CreateEventWithDetailsAsync(CreateEventCompositeDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var eventCreated = await _eventService.CreateAsync(dto.Event, modelState);
        if (!eventCreated)
        {
            modelState.AddModelError("", "Event creation failed.");
            return false;
        }

        var createdEvent = await _eventService.GetAsyncByTitle(dto.Event.Title);
        if (createdEvent == null)
        {
            modelState.AddModelError("", "Could not retrieve the created event.");
            return false;
        }

        foreach (var detailDto in dto.EventDetail)
        {
            detailDto.EventId = createdEvent.Id;
            //detailDto.EventDate = dto.Event.EventDate;

            var existingDetail = await _eventDetailService.GetByEventAndLanguageAsync(createdEvent.Id, detailDto.LanguageId);
            if (existingDetail != null)
            {
                modelState.AddModelError("", $"Event detail for language ID {detailDto.LanguageId} already exists.");
                return false;
            }

            if (!await _eventDetailService.CreateAsync(detailDto, modelState))
            {
                modelState.AddModelError("", $"Failed to create event detail for language ID {detailDto.LanguageId}.");
                return false;
            }
        }
        return true;
    }


    public async Task<bool> UpdateEventWithDetailsAsync(UpdateEventCompositeDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        if (!await _eventService.UpdateAsync(dto.Event, modelState))
            return false;

        foreach (var detailDto in dto.EventDetail)
        {
            if (detailDto.Id > 0)
            {
                if (!await _eventDetailService.UpdateAsync(detailDto, modelState))
                    return false;
            }
            else
            {
                var createDetailDto = _mapper.Map<CreateEventDetailDTO>(detailDto);
                createDetailDto.EventId = dto.Event.Id;
                //createDetailDto.EventDate = dto.Event.EventDate;
                if (!await _eventDetailService.CreateAsync(createDetailDto, modelState))
                    return false;
            }
        }
        return true;
    }

    public async Task<GetEventCompositeDTO> GetByIdAsync(int id)
    {
        return await GetAsync(id);
    }

}
