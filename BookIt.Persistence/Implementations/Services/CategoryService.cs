using AutoMapper;
using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.DTOs.Common;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class CategoryService : ICategoryService
{
    private readonly ILanguageService _languageService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryDetailRepository _categoryDetailRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, ICategoryDetailRepository categoryDetailRepository, IMapper mapper, ILanguageService languageService)
    {
        _categoryRepository = categoryRepository;
        _categoryDetailRepository = categoryDetailRepository;
        _mapper = mapper;
        _languageService = languageService;
    }

    public async Task<GetCategoryDTO> GetAsync(int id, LanguageType language = LanguageType.English)
    {
        var entity = await _categoryRepository.GetAsync(
            id,
            ignoreFilter: false);

        if (entity == null) 
            return null;

        var dto = _mapper.Map<GetCategoryDTO>(entity);

        var details = _categoryDetailRepository
            .GetAll(d => d.CategoryId == entity.Id)
            .ToList();

        dto.CategoryDetails = _mapper.Map<List<GetCategoryDetailDTO>>(details);
        return dto;
    }

    public List<GetCategoryDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var entities = _categoryRepository.GetAll().ToList();
        var dtoList = new List<GetCategoryDTO>();

        foreach (var entity in entities)
        {
            var catDto = _mapper.Map<GetCategoryDTO>(entity);
            // get details
            var details = _categoryDetailRepository
                .GetAll(d => d.CategoryId == entity.Id)
                .ToList();
            catDto.CategoryDetails = _mapper.Map<List<GetCategoryDetailDTO>>(details);
            dtoList.Add(catDto);
        }

        return dtoList;
    }

    public async Task<PaginateDTO<GetCategoryDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _categoryRepository.GetAll();
        var totalItems = await query.CountAsync();

        var paged = _categoryRepository.Paginate(query, limit, page).ToList();

        var items = new List<GetCategoryDTO>();
        foreach (var entity in paged)
        {
            var catDto = _mapper.Map<GetCategoryDTO>(entity);

            var details = _categoryDetailRepository
                .GetAll(d => d.CategoryId == entity.Id)
                .ToList();
            catDto.CategoryDetails = _mapper.Map<List<GetCategoryDetailDTO>>(details);

            items.Add(catDto);
        }

        return new PaginateDTO<GetCategoryDTO>
        {
            Items = items,
            CurrentPage = page,
            PageSize = limit,
            TotalItems = totalItems
        };
    }

    public async Task<bool> IsExistAsync(int id)
    {
        return await _categoryRepository.IsExistAsync(x => x.Id == id && !x.IsDeleted);
    }



    public async Task<bool> CreateAsync(CreateCategoryDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var entity = _mapper.Map<Category>(dto);
        entity.IsDeleted = false;

        await _categoryRepository.CreateAsync(entity);
        await _categoryRepository.SaveChangesAsync();

        if (dto.CategoryDetails != null && dto.CategoryDetails.Any())
        {
            foreach (var detailDto in dto.CategoryDetails)
            {
                var detail = _mapper.Map<CategoryDetail>(detailDto);
                detail.CategoryId = entity.Id; 
                await _categoryDetailRepository.CreateAsync(detail);
            }
            await _categoryDetailRepository.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> UpdateAsync(UpdateCategoryDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var existing = await _categoryRepository.GetAsync(dto.Id);
        if (existing == null)
        {
            modelState.AddModelError("Id", "Category not found.");
            return false;
        }

        existing.Name = dto.Name;
        existing.ParentCategoryId = dto.CategoryId;

        _categoryRepository.Update(existing);
        await _categoryRepository.SaveChangesAsync();

        // Now handle details (if you want them in the same request).
        // This can get tricky: do you want to update existing detail rows, 
        // add new ones, or remove old ones that aren't in the DTO?

        // For a simple approach, let's just update existing detail IDs or insert new:
        if (dto.CategoryDetails != null)
        {
            foreach (var detailDto in dto.CategoryDetails)
            {
                if (detailDto.Id == 0)
                {
                    // brand new detail
                    var newDetail = _mapper.Map<CategoryDetail>(detailDto);
                    newDetail.CategoryId = existing.Id;
                    await _categoryDetailRepository.CreateAsync(newDetail);
                }
                else
                {
                    var existingDetail = await _categoryDetailRepository.GetAsync(detailDto.Id);
                    if (existingDetail == null)
                    {
                        continue;
                    }

                    existingDetail.LanguageId = detailDto.LanguageId;
                    existingDetail.Title = detailDto.Name;
                    _categoryDetailRepository.Update(existingDetail);
                }
            }
            await _categoryDetailRepository.SaveChangesAsync();
        }

        return true;
    }

    public async Task<UpdateCategoryDTO> GetUpdatedDtoAsync(int id)
    {
        var entity = await _categoryRepository.GetAsync(id);
        if (entity == null) 
            return null;

        var dto = _mapper.Map<UpdateCategoryDTO>(entity);

        // gather existing details
        var details = await _categoryDetailRepository
            .GetAll(d => d.CategoryId == entity.Id)
            .ToListAsync();

        dto.CategoryDetails = _mapper.Map<List<UpdateCategoryDetailDTO>>(details);

        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _categoryRepository.GetAsync(id);
        if (existing == null) return;

        _categoryRepository.SoftDelete(existing);
        await _categoryRepository.SaveChangesAsync();

        var details = _categoryDetailRepository.GetAll(d => d.CategoryId == existing.Id).ToList();
        foreach (var detail in details)
        {
            _categoryDetailRepository.SoftDelete(detail);
        }
        await _categoryDetailRepository.SaveChangesAsync();
    }



    public async Task<bool> AddCategoryDetailAsync(CreateCategoryDetailDTO detailDto)
    {
        var categoryExists = await _categoryRepository.IsExistAsync(c => c.Id == detailDto.ParentCategoryId);
        if (!categoryExists) return false;

        var detail = _mapper.Map<CategoryDetail>(detailDto);
        await _categoryDetailRepository.CreateAsync(detail);
        await _categoryDetailRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCategoryDetailAsync(UpdateCategoryDetailDTO detailDto)
    {
        var existing = await _categoryDetailRepository.GetAsync(detailDto.Id);
        if (existing == null) return false;

        existing.LanguageId = detailDto.LanguageId;
        existing.Title = detailDto.Name;
        existing.CategoryId = detailDto.Id; // if changing the category?

        _categoryDetailRepository.Update(existing);
        await _categoryDetailRepository.SaveChangesAsync();
        return true;
    }

    public async Task DeleteCategoryDetailAsync(int detailId)
    {
        var existing = await _categoryDetailRepository.GetAsync(detailId);
        if (existing == null) return;

        _categoryDetailRepository.SoftDelete(existing);
        await _categoryDetailRepository.SaveChangesAsync();
    }


}
