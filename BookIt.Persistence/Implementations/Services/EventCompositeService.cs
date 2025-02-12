using AutoMapper;
using BookIt.Application.DTOs.EventAndDetailsCombinedDTO;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Persistence.Implementations;

public class EventCompositeService : IEventCompositeService
{
    private readonly IEventService _eventService;
    private readonly IEventDetailService _eventDetailService;
    private readonly IMapper _mapper;

    public EventCompositeService(IEventService eventService, IEventDetailService eventDetailService, IMapper mapper)
    {
        _eventService = eventService;
        _eventDetailService = eventDetailService;
        _mapper = mapper;
    }

    public async Task<bool> CreateEventWithDetailsAsync(CreateEventCompositeDTO dto, ModelStateDictionary modelState)
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
            detailDto.EventDate = dto.Event.EventDate;

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

    public async Task<GetEventCompositeDTO?> GetByIdAsync(int id, LanguageType language = LanguageType.English)
    {
        var ev = await _eventService.GetAsync(id, language);
        if (ev == null)
            throw new NotFoundException("Event not found.");

        var details = _eventDetailService.GetAll();
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
                createDetailDto.EventDate = dto.Event.EventDate;
                if (!await _eventDetailService.CreateAsync(createDetailDto, modelState))
                    return false;
            }
        }
        return true;
    }

    public async Task<bool> DeleteAsync(int id, ModelStateDictionary modelState)
    {
        try
        {
            await _eventService.DeleteAsync(id);

            var composite = await GetByIdAsync(id);
            if (composite != null)
            {
                foreach (var detail in composite.EventDetails)
                {
                    await _eventDetailService.DeleteAsync(detail.Id);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            modelState.AddModelError("", ex.Message);
            return false;
        }
    }
  

    public async Task HardDeleteAsync(int id, ModelStateDictionary modelState)
    {
        try
        {
            await _eventService.HardDeleteAsync(id);

            var composite = await GetByIdAsync(id);
            if (composite != null)
            {
                foreach (var detail in composite.EventDetails)
                {
                    await _eventDetailService.HardDeleteAsync(detail.Id);
                }
            }
        }
        catch (Exception ex)
        {
            modelState.AddModelError("", ex.Message);
        }
    }


    public async Task RestoreAsync(int id, ModelStateDictionary modelState)
    {
        try
        {
            await _eventService.RestoreAsync(id);

            var composite = await GetByIdAsync(id);
            if (composite != null)
            {
                foreach (var detail in composite.EventDetails)
                {
                    await _eventDetailService.RestoreAsync(detail.Id);
                }
            }
        }
        catch (Exception ex)
        {
            modelState.AddModelError("", ex.Message);
        }
    }


}
