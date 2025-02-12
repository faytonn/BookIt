//using AutoMapper;
//using BookIt.Application.DTOs.Common;
//using BookIt.Application.DTOs.EventSeatTypeDTO;
//using BookIt.Application.Exceptions;
//using BookIt.Application.Interfaces.Repositories;
//using BookIt.Application.Interfaces.Services;
//using BookIt.Domain.Entities;
//using BookIt.Domain.Enums;
//using BookIt.Persistence.Implementations.Repositories;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore;

//namespace BookIt.Persistence.Implementations.Services;

//public class EventDetailSeatTypeService : IEventDetailSeatTypeService
//{
//    private readonly IEventDetailSeatTypeRepository _repository;
//    private readonly IMapper _mapper;

//    public EventDetailSeatTypeService(IEventDetailSeatTypeRepository repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;
//    }

//    public async Task<GetEventDetailSeatTypeDTO> GetAsync(int id, LanguageType language = LanguageType.English)
//    {
//        var entity = await _repository.GetAsync(x => x.Id == id);
//        if (entity == null)
//            throw new NotFoundException("Event seat type not found.");
//        return _mapper.Map<GetEventDetailSeatTypeDTO>(entity);
//    }

//    public List<GetEventDetailSeatTypeDTO> GetAll(LanguageType language = LanguageType.English)
//    {
//        // You can filter by language if needed.
//        var list = _repository.GetAll().ToList();
//        return _mapper.Map<List<GetEventDetailSeatTypeDTO>>(list);
//    }

//    public async Task<PaginateDTO<GetEventDetailSeatTypeDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
//    {
//        var query = _repository.GetAll();
//        var count = await query.CountAsync();
//        var pageCount = (int)System.Math.Ceiling((decimal)count / limit);
//        if (page > pageCount) page = pageCount;
//        if (page < 1) page = 1;
//        query = _repository.Paginate(query, limit, page);
//        var entities = await query.ToListAsync();
//        var dtos = _mapper.Map<List<GetEventDetailSeatTypeDTO>>(entities);
//        return new PaginateDTO<GetEventDetailSeatTypeDTO>
//        {
//            Items = dtos,
//            CurrentPage = page,
//            PageCount = pageCount,
//        };
//    }

//    public async Task<bool> CreateAsync(CreateEventDetailSeatTypeDTO dto, ModelStateDictionary modelState)
//    {
//        if (!modelState.IsValid)
//            return false;

//        // Check duplicate if needed.
//        var existing = await _repository.GetAsync(x => x.EventDetailId == dto.EventDetailId && x.SeatTypeId == dto.SeatTypeId);

//        if (existing != null)
//        {
//            modelState.AddModelError("", "A seat type for this event detail already exists.");
//            return false;
//        }

//        var entity = _mapper.Map<EventDetailSeatType>(dto);
//        await _repository.CreateAsync(entity);
//        await _repository.SaveChangesAsync();
//        return true;
//    }

//    public async Task<UpdateEventDetailSeatTypeDTO> GetUpdatedDtoAsync(int id)
//    {
//        var entity = await _repository.GetAsync(x => x.Id == id);
//        if (entity == null)
//            throw new NotFoundException("Event seat type not found.");
//        return _mapper.Map<UpdateEventDetailSeatTypeDTO>(entity);
//    }

//    public async Task<bool> UpdateAsync(UpdateEventDetailSeatTypeDTO dto, ModelStateDictionary modelState)
//    {
//        if (!modelState.IsValid)
//            return false;

//        var entity = await _repository.GetAsync(x => x.Id == dto.Id);
//        if (entity == null)
//        {
//            modelState.AddModelError("", "Event seat type not found.");
//            return false;
//        }

//        entity = _mapper.Map(dto, entity);
//        _repository.Update(entity);
//        await _repository.SaveChangesAsync();
//        return true;
//    }

//    public async Task DeleteAsync(int id)
//    {
//        var entity = await _repository.GetAsync(x => x.Id == id);
//        if (entity == null)
//            throw new NotFoundException("Event seat type not found.");
//        _repository.SoftDelete(entity);
//        await _repository.SaveChangesAsync();
//    }

//    public async Task RestoreAsync(int id)
//    {
//        var entity = await _repository.GetAsync(x => x.Id == id, ignoreFilter: true);
//        if (entity == null)
//            throw new NotFoundException("Event seat type not found.");
//        _repository.Repair(entity);
//        await _repository.SaveChangesAsync();
//    }

//    public async Task HardDeleteAsync(int id)
//    {
//        var entity = await _repository.GetAsync(x => x.Id == id, ignoreFilter: true);
//        if (entity == null)
//            throw new NotFoundException("Event seat type not found.");
//        _repository.HardDelete(entity);
//        await _repository.SaveChangesAsync();
//    }

//    public List<GetEventDetailSeatTypeDTO> GetArchivedEventSeatTypes(int eventDetailId)
//    {
//        var archived = _repository.GetAll(ignoreFilter: true)
//                       .Where(x => x.IsDeleted && x.EventDetailId == eventDetailId)
//                       .ToList();
//        return _mapper.Map<List<GetEventDetailSeatTypeDTO>>(archived);
//    }

//    public async Task<bool> IsExistAsync(int id)
//    {
//        return await _repository.IsExistAsync(x => x.Id == id);
//    }
//}
