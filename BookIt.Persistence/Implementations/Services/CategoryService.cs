using AutoMapper;
using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.DTOs.Common;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
        var category = await _categoryRepository.GetAsync(x => x.Id == id, _getWithIncludes());

        if (category == null)
        {
            throw new NotFoundException();
        }

        var dto = _mapper.Map<GetCategoryDTO>(category);

        return dto;
    }

    public  List<GetCategoryDTO> GetAll(LanguageType language = LanguageType.English)
    {
        var categories =  _categoryRepository.GetAll(include: _getWithIncludes(language));



        var dtos = _mapper.Map<List<GetCategoryDTO>>(categories);

        return dtos;
    }

    public async Task<PaginateDTO<GetCategoryDTO>> GetPagesAsync(LanguageType language = LanguageType.English, int page = 1, int limit = 10)
    {
        var query = _categoryRepository.GetAll(include: _getWithIncludes(language));

        var count = query.Count();

        var pageCount = (int)Math.Ceiling((decimal)count / limit);

        if (page > pageCount)
            page = pageCount;

        if (page < 1)
            page = 1;

        query = _categoryRepository.Paginate(query, limit, page);


        var categories = await query.ToListAsync();

        var dtos = _mapper.Map<List<GetCategoryDTO>>(categories);

        PaginateDTO<GetCategoryDTO> paginateDto = new()
        {
            Items = dtos,
            CurrentPage = page,
            PageCount = pageCount
        };

        return paginateDto;
    }





    public async Task<bool> CreateAsync(CreateCategoryDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var extsingCategoryName = await _categoryRepository.GetAsync(x => x.CategoryDetails.FirstOrDefault()!.Name.ToLower() == dto.CategoryDetails.FirstOrDefault()!.Name!.ToLower());

        if (extsingCategoryName != null)
        {
            modelState.AddModelError("", "This category already exists.");
            return false;
        }

        foreach (var detail in dto.CategoryDetails)
        {
            var languageExists = await _languageService.GetLanguageAsync(x => x.Id == detail.LanguageId);

            if (languageExists == null)
            {
                modelState.AddModelError("", "Something went wrong, please try again.");
                return false;
            }
        }

        var category = _mapper.Map<Category>(dto);

        await _categoryRepository.CreateAsync(category);

        await _categoryRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(UpdateCategoryDTO dto, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return false;

        var category = await _categoryRepository.GetAsync(x => x.Id == dto.Id, x => x.Include(x => x.CategoryDetails));

        if (category == null)
            throw new NotFoundException();

        var existingCategory = await _categoryRepository.GetAsync(x => x.CategoryDetails.FirstOrDefault().Name.ToLower() == dto.CategoryDetails.FirstOrDefault()!.Name!.ToLower() && x.Id != dto.Id);

        if(existingCategory != null)
        {
            modelState.AddModelError("", "This category name already exists.");
            return false;
        }

        category = _mapper.Map(dto, category);

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return true;

    }

    public async Task<UpdateCategoryDTO> GetUpdatedDtoAsync(int id)
    {
        var category = await _categoryRepository.GetAsync(x => x.Id == id, _getWithIncludes());

        if (category == null)
            throw new NotFoundException();

        var dto = _mapper.Map<UpdateCategoryDTO>(category);

        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _categoryRepository.GetAsync(id);
        if (exists == null) 
            throw new NotFoundException();

        _categoryRepository.SoftDelete(exists);

        var details = _categoryDetailRepository.GetAll(d => d.CategoryId == exists.Id).ToList();
        foreach (var detail in details)
        {
            _categoryDetailRepository.SoftDelete(detail);
        }
        await _categoryDetailRepository.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var category = await _categoryRepository.GetAsync(id, ignoreFilter: true);
        if (category == null)
            throw new NotFoundException("Category not found.");

        _categoryRepository.Repair(category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task HardDeleteAsync(int id)
    {
        var category = await _categoryRepository.GetAsync(id, ignoreFilter: true);
        if (category == null)
            throw new NotFoundException("Category not found.");

        _categoryRepository.HardDelete(category);
        await _categoryRepository.SaveChangesAsync();
    }


    public List<GetCategoryDTO> GetArchivedCategories(LanguageType language = LanguageType.English)
    {
        var query = _categoryRepository.GetAll(include: _getWithIncludes(language), ignoreFilter: true);

        var archived = query.Where(c => c.IsDeleted).ToList();

        return _mapper.Map<List<GetCategoryDTO>>(archived);
    }

    //public async Task<bool> AddCategoryDetailAsync(CreateCategoryDetailDTO detailDto)
    //{
    //    var categoryExists = await _categoryRepository.IsExistAsync(c => c.Id == detailDto.ParentCategoryId);
    //    if (!categoryExists) return false;

    //    var detail = _mapper.Map<CategoryDetail>(detailDto);
    //    await _categoryDetailRepository.CreateAsync(detail);
    //    await _categoryDetailRepository.SaveChangesAsync();
    //    return true;
    //}

    //public async Task<bool> UpdateCategoryDetailAsync(UpdateCategoryDetailDTO detailDto)
    //{
    //    var existing = await _categoryDetailRepository.GetAsync(detailDto.Id);
    //    if (existing == null) return false;

    //    existing.LanguageId = detailDto.LanguageId;
    //    existing.Name = detailDto.Name;
    //    existing.CategoryId = detailDto.Id; // if changing the category?

    //    _categoryDetailRepository.Update(existing);
    //    await _categoryDetailRepository.SaveChangesAsync();
    //    return true;
    //}

    //public async Task DeleteCategoryDetailAsync(int detailId)
    //{
    //    var existing = await _categoryDetailRepository.GetAsync(detailId);
    //    if (existing == null) return;

    //    _categoryDetailRepository.SoftDelete(existing);
    //    await _categoryDetailRepository.SaveChangesAsync();
    //}


    public async Task<bool> IsExistAsync(int id)
    {
        return await _categoryRepository.IsExistAsync(x => x.Id == id);
    }
    private static Func<IQueryable<Category>, IIncludableQueryable<Category, object>> _getWithIncludes(LanguageType language)
    {
        return x => x.Include(x => x.CategoryDetails.Where(x => x.LanguageId == (int)language));
    }
    private static Func<IQueryable<Category>, IIncludableQueryable<Category, object>> _getWithIncludes()
    {
        return x => x.Include(x => x.CategoryDetails);
    }

   
}
