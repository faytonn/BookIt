using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using BookIt.Domain.Enums;

namespace BookIt.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IEventService _eventService;
    private readonly ISeatService _seatService;
    private readonly ISeatTypeService _seatTypeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationController(
        IReservationService reservationService, 
        IEventService eventService,
        ISeatService seatService,
        ISeatTypeService seatTypeService,
        UserManager<ApplicationUser> userManager)
    {
        _reservationService = reservationService;
        _eventService = eventService;
        _seatService = seatService;
        _seatTypeService = seatTypeService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var reservations = _reservationService.GetAll();
        return View(reservations);
    }

    public IActionResult Cancelled()
    {
        var reservations = _reservationService.GetAll()
            .Where(r => r.Status == ReservationStatus.Cancelled)
            .ToList();
        return View("Index", reservations);
    }

    public async Task<IActionResult> Create()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        var model = new CreateReservationDTO
        {
            UserId = currentUser.Id
        };

        PopulateEventsViewBag();
        PopulateSeatsViewBag();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReservationDTO model)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        // Ensure the UserId matches the current user
        model.UserId = currentUser.Id;

        if (!ModelState.IsValid)
        {
            PopulateEventsViewBag();
            PopulateSeatsViewBag();
            return View(model);
        }

        if (await _reservationService.CreateAsync(model, ModelState))
        {
            return RedirectToAction(nameof(Index));
        }

        PopulateEventsViewBag();
        PopulateSeatsViewBag();
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _reservationService.GetAsync(id);
        if (reservation == null)
            return NotFound();

        PopulateEventsViewBag();
        PopulateStatusesViewBag();
        return View(reservation);
    }

    public async Task<IActionResult> Update(int id)
    {
        var reservation = await _reservationService.GetUpdatedDtoAsync(id);
        if (reservation == null)
            return NotFound();

        PopulateEventsViewBag();
        PopulateStatusesViewBag();
        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateReservationDTO model)
    {
        if (!ModelState.IsValid)
        {
            PopulateEventsViewBag();
            PopulateStatusesViewBag();
            return View(model);
        }

        if (await _reservationService.UpdateAsync(model, ModelState))
        {
            return RedirectToAction(nameof(Index));
        }

        PopulateEventsViewBag();
        PopulateStatusesViewBag();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int id)
    {
        if (await _reservationService.ConfirmReservationAsync(id, null))
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        if (await _reservationService.CancelReservationAsync(id))
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _reservationService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableSeats(int eventId)
    {
        var eventEntity = await _eventService.GetAsync(eventId);
        if (eventEntity == null)
            return NotFound("Event not found.");

        var availableSeats = await _seatService.GetAvailableSeatsForEventAsync(eventId);
        if (availableSeats == null || !availableSeats.Any())
        {
            return Json(new List<object>());
        }

        var seatTypes = _seatTypeService.GetAll();
        var seatTypeDictionary = seatTypes?
            .ToDictionary(st => st.Id, st => st);

        return Json(availableSeats.Select(s => {
            decimal price = 0;
            if (seatTypeDictionary != null && seatTypeDictionary.TryGetValue(s.SeatTypeId, out var seatType))
            {
                price = seatType.DefaultPrice;
            }
            return new { 
                id = s.Id, 
                name = s.SeatName,
                row = s.SeatRow,
                column = s.SeatColumn,
                price = price
            };
        }));
    }

    private void PopulateEventsViewBag()
    {
        ViewBag.Events = new SelectList(_eventService.GetAll() ?? Enumerable.Empty<object>(), "Id", "Title");
    }

    private void PopulateSeatsViewBag()
    {
        ViewBag.Seats = new SelectList(new List<object>());
    }

    private void PopulateStatusesViewBag()
    {
        ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(ReservationStatus))
            .Cast<ReservationStatus>()
            .Select(s => new { Id = (int)s, Name = s.ToString() }), "Id", "Name");
    }
} 