using AutoMapper;
using BookIt.Application.DTOs.EventAndDetailsCombinedDTO;
using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
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
        ViewData["GeneralLocations"] = new SelectList(_locationService.GetAll(), "Id", "Name");
        ViewData["Categories"] = new SelectList(_categoryService.GetAll(LanguageType.English), "Id", "Name");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEventDTO dto)
    {
        ViewData["GeneralLocations"] = new SelectList(_locationService.GetAll(), "Id", "Name", dto.GeneralLocationId);
        ViewData["Categories"] = new SelectList(_categoryService.GetAll(LanguageType.English), "Id", "Name", dto.CategoryId);

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
            var uploadUrl = await _cloudinaryService.FileCreateAsync(dto.ImageFile);
            dto.ImagePath = uploadUrl;
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

        ViewData["GeneralLocations"] = new SelectList(_locationService.GetAll(), "Id", "Name", dto.GeneralLocationId);
        ViewData["Categories"] = new SelectList(_categoryService.GetAll(LanguageType.English), "Id", "Name", dto.CategoryId);
        ViewBag.Halls = _hallService.GetAll().Where(h => h.LocationId == dto.GeneralLocationId).ToList();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateEventDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["GeneralLocations"] = new SelectList(_locationService.GetAll(), "Id", "Name", dto.GeneralLocationId);
            ViewData["Categories"] = new SelectList(_categoryService.GetAll(LanguageType.English), "Id", "Name", dto.CategoryId);
            ViewBag.Halls = _hallService.GetAll().Where(h => h.LocationId == dto.GeneralLocationId).ToList();
            return View(dto);
        }

        if (dto.ImageFile != null)
        {
            var uploadUrl = await _cloudinaryService.FileCreateAsync(dto.ImageFile);
            dto.ImagePath = uploadUrl;
        }

        var result = await _eventService.UpdateAsync(dto, ModelState);
        if (!result)
        {
            ViewData["GeneralLocations"] = new SelectList(_locationService.GetAll(), "Id", "Name", dto.GeneralLocationId);
            ViewData["Categories"] = new SelectList(_categoryService.GetAll(LanguageType.English), "Id", "Name", dto.CategoryId);
            ViewBag.Halls = _hallService.GetAll().Where(h => h.LocationId == dto.GeneralLocationId).ToList();
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

    private void PopulateDropdowns()
    {
        var locations = _locationService.GetAll()
            .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
            .ToList();
        ViewBag.Locations = locations;

    }
}

