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

    public EventDetailController(IEventDetailService eventDetailService, IHallService hallService)
    {
        _eventDetailService = eventDetailService;
        _hallService = hallService;
    }

    public IActionResult Index(int eventId, int languageId = 1)
    {
        var details =  _eventDetailService.GetAll();
        details = details.Where(d => d.LanguageId == languageId).ToList();

        ViewBag.EventId = eventId;
        ViewBag.LanguageId = languageId;
        return View(details);
    }

    public IActionResult Create(int eventId, int languageId = 1)
    {
        ViewBag.EventId = eventId;
        ViewBag.LanguageId = languageId;
        PopulateDropdowns(); // Populate dropdown for Halls, etc.
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEventDetailDTO dto)
    {
        // Handle image upload (if a file is posted)
        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            dto.ImagePath = "/uploads/" + fileName;
        }

        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }

        var result = await _eventDetailService.CreateAsync(dto, ModelState);
        if (!result)
        {
            PopulateDropdowns();
            return View(dto);
        }
        return RedirectToAction("Index", new { eventId = dto.EventId, languageId = dto.LanguageId });
    }

    // GET: /Admin/EventDetail/Update/5
    public async Task<IActionResult> Update(int id)
    {
        var dto = await _eventDetailService.GetUpdatedDtoAsync(id);
        PopulateDropdowns();
        return View(dto);
    }

    // POST: /Admin/EventDetail/Update/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateEventDetailDTO dto)
    {
        // Handle image upload if a new file is provided.
        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            dto.ImagePath = "/uploads/" + fileName;
        }

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

    // POST: /Admin/EventDetail/Delete?id=5&eventId=1&languageId=1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int eventId, int languageId)
    {
        await _eventDetailService.DeleteAsync(id);
        return RedirectToAction("Index", new { eventId, languageId });
    }

    // GET: /Admin/EventDetail/Archived?eventId=1&languageId=1
    public IActionResult Archived(int eventId, int languageId = 1)
    {
        var archived = _eventDetailService.GetArchivedEventDetails(eventId, (Domain.Enums.LanguageType)languageId);
        ViewBag.EventId = eventId;
        ViewBag.LanguageId = languageId;
        return View(archived);
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

    private void PopulateDropdowns()
    {
        // Example: Populate halls dropdown from HallService
        var halls = _hallService.GetAll()
            .Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Name })
            .ToList();
        ViewBag.Halls = halls;

        // Optionally populate other dropdowns if needed.
    }
}
