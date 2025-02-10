using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Areas.Admin.ViewComponents;

public class SoftDeletedCategoriesViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public SoftDeletedCategoriesViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var allCategories = _categoryService.GetAll(language: LanguageType.English);
        var softDeletedCategories = allCategories.Where(c => c.IsDeleted).ToList();

        return View(softDeletedCategories);
    }
}
