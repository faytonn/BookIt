//using BookIt.Application.DTOs.EventSeatTypeDTO;
//using BookIt.Application.Interfaces.Services;
//using BookIt.Domain.Enums;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BookIt.Presentation.Areas.Admin.Controllers;

//[Area("Admin")]
//public class EventSeatTypeController : Controller
//{
//    private readonly IEventDetailSeatTypeService _eventSeatTypeService;

//    public EventSeatTypeController(IEventDetailSeatTypeService eventSeatTypeService)
//    {
//        _eventSeatTypeService = eventSeatTypeService;
//    }

//    // GET: /Admin/EventSeatType/Index?eventDetailId=1
//    public IActionResult Index(int eventDetailId)
//    {
//        var list = _eventSeatTypeService.GetAll();

//        list = list.Where(x => x.EventId == eventDetailId).ToList();
//        ViewBag.EventDetailId = eventDetailId;
//        return View(list);
//    }

//    public IActionResult Create(int eventDetailId)
//    {
//        ViewBag.EventDetailId = eventDetailId;
//        return View();
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create(CreateEventDetailSeatTypeDTO dto)
//    {
//        if (!ModelState.IsValid)
//            return View(dto);

//        var result = await _eventSeatTypeService.CreateAsync(dto, ModelState);
//        if (!result)
//            return View(dto);

//        return RedirectToAction("Index", new { eventDetailId = dto.EventDetailId });
//    }

//    public async Task<IActionResult> Update(int id)
//    {
//        var dto = await _eventSeatTypeService.GetUpdatedDtoAsync(id);
//        return View(dto);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Update(int id, UpdateEventDetailSeatTypeDTO dto)
//    {
//        if (!ModelState.IsValid)
//            return View(dto);

//        var result = await _eventSeatTypeService.UpdateAsync(dto, ModelState);
//        if (!result)
//            return View(dto);

//        return RedirectToAction("Index", new { eventDetailId = dto.Id });
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Delete(int id, int eventDetailId)
//    {
//        await _eventSeatTypeService.DeleteAsync(id);
//        return RedirectToAction("Index", new { eventDetailId });
//    }

//    public IActionResult Archived(int eventDetailId)
//    {
//        var archived = _eventSeatTypeService.GetArchivedEventSeatTypes(eventDetailId);
//        ViewBag.EventDetailId = eventDetailId;
//        return View(archived);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Restore(int id, int eventDetailId)
//    {
//        await _eventSeatTypeService.RestoreAsync(id);
//        return RedirectToAction("Archived", new { eventDetailId });
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> HardDelete(int id, int eventDetailId)
//    {
//        await _eventSeatTypeService.HardDeleteAsync(id);
//        return RedirectToAction("Archived", new { eventDetailId });
//    }
//}
