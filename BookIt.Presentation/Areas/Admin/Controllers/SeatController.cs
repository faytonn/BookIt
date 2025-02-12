using BookIt.Application.DTOs.HallDTO;
using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.DTOs.SeatTypeDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Web.Controllers;

[Area("Admin")]
public class SeatController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IHallService _hallService;
    private readonly ISeatTypeService _seatTypeService;
    private readonly IGeneralLocationService _generalLocationService;


    public SeatController(ISeatService seatService, IHallService hallService, ISeatTypeService seatTypeService, IGeneralLocationService generalLocationService)
    {
        _seatService = seatService;
        _hallService = hallService;
        _seatTypeService = seatTypeService;
        _generalLocationService = generalLocationService;
    }

    public async Task<IActionResult> Index(int generalLocationId = 0, int hallId = 0, int seatTypeId = 0)
    {
        var model = new SeatFilterDTO();

        // Populate General Locations
        var locations = _generalLocationService.GetAll(); // returns List<GetGeneralLocationDTO> with Id and Name
        model.GeneralLocations = new SelectList(locations, "Id", "Name", generalLocationId);

        // Populate halls if a general location is selected
        if (generalLocationId > 0)
        {
            var halls = _hallService.GetAll()
                                    .Where(h => h.LocationId == generalLocationId)
                                    .ToList();
            model.Halls = new SelectList(halls, "Id", "Name", hallId);
        }
        else
        {
            model.Halls = new List<SelectListItem>();
        }

        // Populate seat types and seats if a hall is selected
        if (hallId > 0)
        {
            var seatTypes = await _seatTypeService.GetByHall(hallId);
            model.SeatTypes = new SelectList(seatTypes, "Id", "Name", seatTypeId);

            var seats = await _seatService.GetSeatsByHallAsync(hallId);
            if (seatTypeId > 0)
            {
                seats = seats.Where(s => s.SeatTypeId == seatTypeId).ToList();
            }
            model.Seats = seats;
        }
        else
        {
            model.SeatTypes = new List<SelectListItem>();
            model.Seats = new List<GetSeatDTO>();
        }

        // Set selected filter values
        model.SelectedGeneralLocationId = generalLocationId;
        model.SelectedHallId = hallId;
        model.SelectedSeatTypeId = seatTypeId;

        return View(model);
    }

    public async Task<IActionResult> GetSeatTypesByHall(int hallId)
    {
        var seatTypes = await _seatTypeService.GetByHall(hallId);
        return Json(seatTypes);
    }

    public IActionResult Create(int hallId)
    {
        PopulateHallsAndSeatTypes();
        var dto = new CreateSeatDTO { HallId = hallId };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSeatDTO dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _seatService.CreateAsync(dto, ModelState);
            if (result)
                return RedirectToAction("Index", new { hallId = dto.HallId });
        }
        PopulateHallsAndSeatTypes();
        return View(dto);
    }

    public IActionResult BulkCreate(int hallId)
    {
        PopulateHallsAndSeatTypes();
        var dto = new CreateBulkSeatDTO { HallId = hallId };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkCreate(CreateBulkSeatDTO dto)
    {
        Console.WriteLine($"BulkCreate DTO - HallId: {dto.HallId}, SeatTypeId: {dto.SeatTypeId}, StartRow: {dto.StartRow}, EndRow: {dto.EndRow}, StartColumn: {dto.StartColumn}, EndColumn: {dto.EndColumn}");

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("ModelState Error: " + error.ErrorMessage);
            }
            PopulateHallsAndSeatTypes();
            return View(dto);
        }
        try
        {
            var result = await _seatService.BulkCreateSeatsAsync(dto, ModelState);
            if (result)
                return RedirectToAction("Index", new { hallId = dto.HallId });
            else
            {
                PopulateHallsAndSeatTypes();
                return View(dto);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            PopulateHallsAndSeatTypes();
            return View(dto);
        }

    }

    public async Task<IActionResult> Update(int id)
    {
        var dto = await _seatService.GetUpdatedDtoAsync(id);
        if (dto == null)
            return NotFound();

        PopulateHallsAndSeatTypes();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateSeatDTO dto)
    {
        if (ModelState.IsValid)
        {
            await _seatService.UpdateAsync(dto, ModelState);
            return RedirectToAction("Index", new { hallId = dto.HallId });
        }
        PopulateHallsAndSeatTypes();
        return View(dto);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, int hallId)
    {
        await _seatService.DeleteAsync(id);
        return RedirectToAction("Index", new { hallId });
    }

    public IActionResult Archived(LanguageType language = LanguageType.English)
    {
        var archivedSeats = _seatService.GetArchivedSeats(language);
        return View(archivedSeats);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _seatService.RestoreAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HardDelete(int id)
    {
        await _seatService.HardDeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    private void PopulateHallsAndSeatTypes()
    {
        var halls = _hallService.GetAll();
        var seatTypes = _seatTypeService.GetAll();


        halls.Insert(0, new GetHallDTO { Id = 0, Name = "-- Please select a hall --" });
        seatTypes.Insert(0, new GetSeatTypeDTO { Id = 0, Name = "-- Please select a seat type --" });


        ViewBag.Halls = new SelectList(halls, "Id", "Name");
        ViewBag.SeatTypes = new SelectList(seatTypes, "Id", "Name");
    }
}
