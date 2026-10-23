using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using BookIt.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Controllers;

public class EventController : Controller
{
    private readonly IEventService _eventService;
    private readonly IEventDetailService _eventDetailService;
    private readonly IGeneralLocationService _locationService;
    private readonly ICategoryService _categoryService;
    private readonly ICookieService _cookieService;

    public EventController(
        IEventService eventService,
        IEventDetailService eventDetailService,
        IGeneralLocationService locationService,
        ICategoryService categoryService,
        ICookieService cookieService)
    {
        _eventService = eventService;
        _eventDetailService = eventDetailService;
        _locationService = locationService;
        _categoryService = categoryService;
        _cookieService = cookieService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(EventFilterDTO filter)
    {
        var language = await _cookieService.GetSelectedLanguageTypeAsync();

        var model = new EventListViewModel
        {
            Filter = filter,
            Events = await _eventService.SearchAsync(filter, language)
        };
        _populateDropdowns(model);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var language = await _cookieService.GetSelectedLanguageTypeAsync();
        var ev = await _eventService.GetAsync(id, language);

        // prefer the visitor's language, fall back to English, then to whatever exists
        var details = await _eventDetailService.GetAllByEventId(id);
        var detail = details.FirstOrDefault(d => d.LanguageId == (int)language)
                     ?? details.FirstOrDefault(d => d.LanguageId == (int)LanguageType.English)
                     ?? details.FirstOrDefault();

        return View(new EventDetailsViewModel { Event = ev, Detail = detail });
    }

    private void _populateDropdowns(EventListViewModel model)
    {
        model.Locations = _locationService.GetAll()
            .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name, Selected = l.Id == model.Filter.LocationId })
            .ToList();

        model.Categories = _categoryService.GetAll()
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == model.Filter.CategoryId })
            .ToList();
    }
}
