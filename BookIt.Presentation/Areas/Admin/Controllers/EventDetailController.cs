using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers;


[Area("Admin")]
public class EventDetailController : Controller
{
    private readonly IEventDetailService _eventDetailService;
    private readonly IHallService _hallService;
    private readonly IEventService _eventService;

    public EventDetailController(IEventDetailService eventDetailService, IHallService hallService, IEventService eventService)
    {
        _eventDetailService = eventDetailService;
        _hallService = hallService;
        _eventService = eventService;
    }

    public async Task<IActionResult> Index(int? eventId = null, int languageId = 1)
    {
        if (!eventId.HasValue)
        {
            var events = _eventService.GetAll();
            return View("SelectEvent", events);
        }

        try
        {
            var details = await _eventDetailService.GetAllByEventId(eventId.Value);
            var filteredDetails = details.Where(d => d.LanguageId == languageId).ToList();

            ViewBag.EventId = eventId;
            ViewBag.LanguageId = languageId;
            var eventData = await _eventService.GetAsync(eventId.Value);
            ViewBag.EventTitle = eventData?.Title ?? "Unknown Event";
            ViewBag.Languages = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "English", Selected = languageId == 1 },
                new SelectListItem { Value = "2", Text = "Azerbaijani", Selected = languageId == 2 },
                new SelectListItem { Value = "3", Text = "Czech", Selected = languageId == 3 }
            };

            //TempData["DebugInfo"] = $"Found {filteredDetails.Count} details for event {eventId} in language {languageId}";
            
            return View("Index", filteredDetails);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
            return RedirectToAction("Index", "Event");
        }
    }

    public IActionResult Create(int eventId, int languageId = 1)
    {
        try
        {
            var eventTitle = _eventService.GetAsync(eventId).Result?.Title ?? "Unknown Event";
            ViewBag.EventId = eventId;
            ViewBag.LanguageId = languageId;
            ViewBag.EventTitle = eventTitle;
            ViewBag.Languages = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "English" },
                new SelectListItem { Value = "2", Text = "Azerbaijani" },
                new SelectListItem { Value = "3", Text = "Czech" }
            };
            PopulateDropdowns(); 
            return View();
        }
        catch (Exception)
        {
            TempData["Error"] = "The specified event could not be found.";
            return RedirectToAction("Index", "Event");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEventDetailDTO dto)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                TempData["Error"] = error.ErrorMessage;
            }
            PopulateDropdowns();
            return View(dto);
        }

        var result = await _eventDetailService.CreateAsync(dto, ModelState);
        if (!result)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                TempData["Error"] = error.ErrorMessage;
            }
            PopulateDropdowns();
            return View(dto);
        }

        TempData["Success"] = "Event detail created successfully.";
        return RedirectToAction("Index", new { eventId = dto.EventId, languageId = dto.LanguageId });
    }

    public async Task<IActionResult> Update(int id)
    {
        var dto = await _eventDetailService.GetUpdatedDtoAsync(id);
        PopulateDropdowns();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateEventDetailDTO dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }

        var result = await _eventDetailService.UpdateAsync(dto, ModelState);
        if (!result)
        {
            PopulateDropdowns();
            return View(dto);
        }

        return RedirectToAction("Index", new { eventId = dto.EventId, languageId = dto.LanguageId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int eventId, int languageId)
    {
        await _eventDetailService.DeleteAsync(id);
        return RedirectToAction("Index", new { eventId, languageId });
    }

    public IActionResult Archived(int eventId)
    {
        //var archived = _eventDetailService.GetArchivedEventDetails(eventId);
        //ViewBag.EventId = eventId;
        //return View(archived);
        var archived = _eventDetailService.GetAllArchivedEventDetails();
        return View("Archived", archived);


    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id, int eventId, int languageId)
    {
        await _eventDetailService.RestoreAsync(id);
        return RedirectToAction("Archived", new { eventId, languageId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDelete(int id, int eventId, int languageId)
    {
        await _eventDetailService.HardDeleteAsync(id);
        return RedirectToAction("Archived", new { eventId, languageId });
    }

    //public IActionResult AllArchived()
    //{
    //    var archived = _eventDetailService.GetAllArchivedEventDetails();
    //    return View("Archived", archived);
    //}

    private void PopulateDropdowns()
    {
        var halls = _hallService.GetAll()
            .Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Name })
            .ToList();
        ViewBag.Halls = halls;

    }
}
