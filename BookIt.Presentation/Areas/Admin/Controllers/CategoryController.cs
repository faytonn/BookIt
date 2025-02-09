using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize(Roles = "Admin, Moderator")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService) 
    {
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        var dtos = _categoryService.GetAll(language: LanguageType.English);

        return View(dtos);
    }

    public IActionResult Create() 
    {
        return View();  
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryDTO dto)
    {
        var result = await _categoryService.CreateAsync(dto, ModelState);

        if (!result)
            return View(dto);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var country = await _categoryService.GetUpdatedDtoAsync(id);

        return View(country);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryDTO dto)
    {
        var result = await _categoryService.UpdateAsync(dto, ModelState);
        if (!result)
            return View(dto);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

}
