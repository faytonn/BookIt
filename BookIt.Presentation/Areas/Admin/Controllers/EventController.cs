using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.DTOs.HallDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Domain.Enums;
using BookIt.Persistence.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class EventController : Controller
{
    private readonly IEventService _eventService;
    private readonly IGeneralLocationService _locationService;
    private readonly ICategoryService _categoryService;
    private readonly IHallService _hallService;
    private readonly ICloudinaryService _cloudinaryService;


    public EventController(IEventService eventService, IGeneralLocationService locationService, ICategoryService categoryService, IHallService hallService, ICloudinaryService cloudinaryService)
    {
        _eventService = eventService;
        _locationService = locationService;
        _categoryService = categoryService;
        _hallService = hallService;
        _cloudinaryService = cloudinaryService;
    }

    public IActionResult Index()
    {
        var events = _eventService.GetAll();
        return View(events);
    }

    public IActionResult Create()
    {
        var model = new CreateEventDTO();
        _populateDropdowns();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEventDTO dto)
    {
        _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                TempData["Error"] = error.ErrorMessage;
            }
            return View(dto);
        }

        if (dto.ImageFile != null)
        {
            if (!_isValidImage(dto.ImageFile))
            {
                _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);
                return View(dto);
            }
            try
            {
                dto.ImagePath = await _cloudinaryService.FileCreateAsync(dto.ImageFile);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);
                return View(dto);
            }
        }
        else
        {
            ModelState.AddModelError("ImageFile", "An image file is required.");
            TempData["Error"] = "An image file is required.";
            return View(dto);
        }

        var result = await _eventService.CreateAsync(dto, ModelState);
        if (!result)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                TempData["Error"] = error.ErrorMessage;
            }
            return View(dto);
        }

        TempData["Success"] = "Event created successfully.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var dto = await _eventService.GetUpdatedDtoAsync(id);
        if (dto == null)
            return NotFound();

        _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateEventDTO dto)
    {
        if (!ModelState.IsValid)
        {
            _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);
            return View(dto);
        }

        if (dto.ImageFile != null)
        {
            if (!_isValidImage(dto.ImageFile))
            {
                _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);
                return View(dto);
            }
            try
            {
                dto.ImagePath = await _cloudinaryService.FileCreateAsync(dto.ImageFile);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);
                return View(dto);
            }
        }

        var result = await _eventService.UpdateAsync(dto, ModelState);
        if (!result)
        {
            _populateDropdowns(dto.GeneralLocationId, dto.CategoryId);
            return View(dto);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _eventService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Archived()
    {
        var archived = _eventService.GetArchivedEvents();
        return View(archived);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _eventService.RestoreAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDelete(int id)
    {
        await _eventService.HardDeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult GetHallsByLocation(int generalLocationId)
    {
        var halls = _hallService.GetAll()
            .Where(h => h.LocationId == generalLocationId && !h.IsDeleted)
            .Select(h => new { id = h.Id, name = h.Name })
            .ToList();
        
        Console.WriteLine($"Found {halls.Count} halls for location {generalLocationId}");
        return Json(halls);
    }

    private bool _isValidImage(IFormFile file)
    {
        if (!file.CheckType("image"))
        {
            ModelState.AddModelError("ImageFile", "Only image files are allowed.");
            return false;
        }
        if (!file.CheckSize(2))
        {
            ModelState.AddModelError("ImageFile", "The image must be 2 MB or smaller.");
            return false;
        }
        return true;
    }

    private void _populateDropdowns(int? selectedLocationId = null, int? selectedCategoryId = null)
    {
        ViewData["GeneralLocations"] = new SelectList(_locationService.GetAll(), "Id", "Name", selectedLocationId);
        ViewData["Categories"] = new SelectList(_categoryService.GetAll(LanguageType.English), "Id", "Name", selectedCategoryId);
        ViewBag.Halls = selectedLocationId.HasValue
            ? _hallService.GetAll().Where(h => h.LocationId == selectedLocationId.Value).ToList()
            : new List<GetHallDTO>();
    }
}

