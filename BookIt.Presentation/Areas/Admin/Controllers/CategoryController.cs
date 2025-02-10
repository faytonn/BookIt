using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        var categories = _categoryService.GetAll(language: LanguageType.English)
                                .Where(c => c.ParentCategoryId == null)
                                .Select(c => new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    // You can display a language-specific name or a fallback
                                    Text = c.CategoryDetails.FirstOrDefault()?.Name ?? c.Name
                                })
                                .ToList();

        // Insert a "None" option at the top.
        categories.Insert(0, new SelectListItem { Value = "", Text = "None" });
        ViewBag.ParentCategories = categories;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryDTO dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateParentCategories();
            return View(dto);
        }

        var result = await _categoryService.CreateAsync(dto, ModelState);
        if (!result)
        {
            PopulateParentCategories();
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var country = await _categoryService.GetUpdatedDtoAsync(id);

        ViewBag.Languages = GetLanguages();

        return View(country);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryDTO dto)
    {
        var result = await _categoryService.UpdateAsync(dto, ModelState);

        if (!result)
        {
            ViewBag.Languages = GetLanguages();
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Archived()
    {
        var archived = _categoryService.GetArchivedCategories(LanguageType.English);

        var allCategories = _categoryService.GetAll(LanguageType.English);

        ViewBag.AllCategories = allCategories;
        return View(archived);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _categoryService.RestoreAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDelete(int id)
    {
        await _categoryService.HardDeleteAsync(id); 
        return RedirectToAction(nameof(Index));
    }


    private List<SelectListItem> GetLanguages()
    {
        return new List<SelectListItem>
        {
        new SelectListItem { Value = ((int)LanguageType.English).ToString(), Text = "English" },
        new SelectListItem { Value = ((int)LanguageType.Azerbaijani).ToString(), Text = "Azerbaijani" },
        new SelectListItem { Value = ((int) LanguageType.Czech).ToString(), Text = "Czech" },
        };
    }

    private void PopulateParentCategories()
    {
        var categories = _categoryService.GetAll(language: LanguageType.English)
                                .Where(c => c.ParentCategoryId == null)
                                .Select(c => new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    Text = c.CategoryDetails.FirstOrDefault()?.Name ?? c.Name
                                })
                                .ToList();

        categories.Insert(0, new SelectListItem { Value = "", Text = "None" });
        ViewBag.ParentCategories = categories;
    }
}
