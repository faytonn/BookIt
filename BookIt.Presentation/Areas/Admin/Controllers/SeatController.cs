using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers;

public class SeatController : Controller
{
    private readonly ISeatService _seatService;
    private readonly ISeatTypeService _seatTypeService;
    private readonly IHallService _hallService;

    public SeatController(ISeatService seatService, ISeatTypeService seatTypeService, IHallService hallService)
    {
        _seatService = seatService;
        _seatTypeService = seatTypeService;
        _hallService = hallService;
    }

    public IActionResult Index()
    {
        var seats = _seatService.GetAll(); 
        return View(seats);
    }
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSeatDTO dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }

        var result = await _seatService.CreateAsync(dto, ModelState);
        if (!result)
        {
            PopulateDropdowns();
            return View(dto);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var dto = await _seatService.GetUpdatedDtoAsync(id);
        if (dto == null)
            return NotFound();
        PopulateDropdowns();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateSeatDTO dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }

        var result = await _seatService.UpdateAsync(dto, ModelState);
        if (!result)
        {
            PopulateDropdowns();
            return View(dto);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _seatService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private void PopulateDropdowns()
    {
        var seatTypes = _seatTypeService.GetAll()
             .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
        ViewBag.SeatTypes = seatTypes;

        var halls = _hallService.GetAll()
             .Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Name }).ToList();
        ViewBag.Halls = halls;

    }
}
