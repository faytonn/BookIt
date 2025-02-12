using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class SeatTypeController : Controller
{
    private readonly ISeatTypeService _seatTypeService;
    private readonly IHallService _hallService;

    public SeatTypeController(ISeatTypeService seatTypeService, IHallService hallService)
    {
        _seatTypeService = seatTypeService;
        _hallService = hallService;
    }

    public IActionResult Index()
    {
        var seatTypes = _seatTypeService.GetAll();
        return View(seatTypes);
    }

    public IActionResult Create()
    {
        var halls = _hallService.GetAll()
        .Select(h => new SelectListItem
        {
            Value = h.Id.ToString(),
            Text = h.Name
        })
        .ToList();

        ViewBag.Halls = halls;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSeatTypeDTO dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }

        var result = await _seatTypeService.CreateAsync(dto, ModelState);
        if (!result)
        {
            PopulateDropdowns();
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var dto = await _seatTypeService.GetUpdatedDtoAsync(id);
        if (dto == null)
        {
            return NotFound();
        }
        PopulateDropdowns();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateSeatTypeDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            PopulateDropdowns();
            return View(dto);
        }

        var result = await _seatTypeService.UpdateAsync(dto, ModelState);
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
        await _seatTypeService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Archived()
    {
        var archived = _seatTypeService.GetArchivedSeatTypes();
        return View(archived);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _seatTypeService.RestoreAsync(id);
        return RedirectToAction(nameof(Archived));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDelete(int id)
    {
        await _seatTypeService.HardDeleteAsync(id);
        return RedirectToAction(nameof(Archived));
    }

    private void PopulateDropdowns()
    {
        var halls = _hallService.GetAll()
            .Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Name })
            .ToList();
        ViewBag.Halls = halls;
    }
}
