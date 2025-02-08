using BookIt.Application.DTOs.CategoryDetailDTO;
using BookIt.Application.DTOs.CategoryDTO;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _categoryService.CreateAsync(dto, ModelState);
        if (!success) return BadRequest(ModelState);

        return Ok("Category created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _categoryService.UpdateAsync(dto, ModelState);

        if (!success) 
            return BadRequest(ModelState);

        return Ok("Category updated successfully.");
    }

    [HttpPost("AddDetail")]
    public async Task<IActionResult> AddCategoryDetail(CreateCategoryDetailDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _categoryService.AddCategoryDetailAsync(dto);
        if (!success) return NotFound("Category does not exist.");

        return Ok("Category detail added.");
    }

    [HttpPut("UpdateDetail")]
    public async Task<IActionResult> UpdateCategoryDetail(UpdateCategoryDetailDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _categoryService.UpdateCategoryDetailAsync(dto);
        if (!success) return NotFound("Category detail not found.");

        return Ok("Category detail updated.");
    }

    [HttpDelete("Detail/{detailId}")]
    public async Task<IActionResult> DeleteCategoryDetail(int detailId)
    {
        await _categoryService.DeleteCategoryDetailAsync(detailId);
        return Ok("Category detail deleted.");
    }
}
