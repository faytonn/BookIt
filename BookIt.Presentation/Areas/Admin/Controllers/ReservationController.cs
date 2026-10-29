using BookIt.Application.DTOs.ReservationDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BookIt.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IEventService _eventService;
    private readonly ISeatService _seatService;
    private readonly IEventSeatService _eventSeatService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationController(
        IReservationService reservationService,
        IEventService eventService,
        ISeatService seatService,
        IEventSeatService eventSeatService,
        UserManager<ApplicationUser> userManager)
    {
        _reservationService = reservationService;
        _eventService = eventService;
        _seatService = seatService;
        _eventSeatService = eventSeatService;
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

    public IActionResult Create()
    {
        PopulateEventsViewBag();
        PopulateSeatsViewBag();
        return View(new CreateReservationDTO());
    }

    // Box-office sale: the admin books seats for a customer (found by email), not for themselves
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReservationDTO model)
    {
        ModelState.Remove(nameof(CreateReservationDTO.UserId));
        if (ModelState.IsValid)
        {
            var customer = await _userManager.FindByEmailAsync(model.CustomerEmail.Trim());
            if (customer == null)
                ModelState.AddModelError(nameof(CreateReservationDTO.CustomerEmail), "No customer account with this email. Ask them to register first.");
            else
                model.UserId = customer.Id;
        }

        if (ModelState.IsValid && await _reservationService.CreateAsync(model, ModelState))
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

    // The old edit form could move a booking to another event (its seats stayed in the old hall)
    // and set "Cancelled" without freeing seats. Every change now goes through Details.
    public IActionResult Update(int id)
    {
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int id)
    {
        if (await _reservationService.ConfirmReservationAsync(id, null))
        {
            return RedirectToAction(nameof(Details), new { id });
        }
        ModelState.AddModelError("", "Only a pending reservation can be confirmed.");
        return View(nameof(Details), await _reservationService.GetAsync(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        if (await _reservationService.CancelReservationAsync(id, ModelState))
        {
            return RedirectToAction(nameof(Details), new { id });
        }
        return View(nameof(Details), await _reservationService.GetAsync(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkRefunded(int id)
    {
        if (await _reservationService.MarkRefundedAsync(id, ModelState))
        {
            return RedirectToAction(nameof(Details), new { id });
        }
        return View(nameof(Details), await _reservationService.GetAsync(id));
    }

    public async Task<IActionResult> AddSeats(int id)
    {
        var reservation = await _reservationService.GetAsync(id);
        await _populateAvailableSeatsAsync(reservation.EventId);
        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSeats(int id, List<int> seatIds)
    {
        if (seatIds.Count == 0)
            ModelState.AddModelError("", "Pick at least one seat.");
        else if (await _reservationService.AddSeatsToReservationAsync(id, seatIds))
            return RedirectToAction(nameof(Details), new { id });
        else
            ModelState.AddModelError("", "Those seats can't be added: one was taken a moment ago, or the reservation is cancelled.");

        var reservation = await _reservationService.GetAsync(id);
        await _populateAvailableSeatsAsync(reservation.EventId);
        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveSeats(int id, List<int> seatIds)
    {
        await _reservationService.RemoveSeatsFromReservationAsync(id, seatIds);
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task _populateAvailableSeatsAsync(int eventId)
    {
        var seatMap = await _eventSeatService.GetSeatMapAsync(eventId);
        ViewBag.AvailableSeats = seatMap
            .Where(s => s.Status == EventSeatStatus.Available)
            .OrderBy(s => s.Row).ThenBy(s => s.Column)
            .ToList();
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

        var seatMap = await _eventSeatService.GetSeatMapAsync(eventId);

        return Json(seatMap
            .Where(s => s.Status == EventSeatStatus.Available)
            .Select(s => new
            {
                id = s.SeatId,
                name = s.SeatName,
                row = s.Row,
                column = s.Column,
                price = s.Price
            }));
    }

    private void PopulateEventsViewBag()
    {
        ViewBag.Events = _eventService.GetAll()
            .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Title })
            .ToList();
    }

    private void PopulateSeatsViewBag()
    {
        ViewBag.Seats = new List<SelectListItem>();
    }

    private void PopulateStatusesViewBag()
    {
        ViewBag.Statuses = Enum.GetValues(typeof(ReservationStatus))
            .Cast<ReservationStatus>()
            .Select(s => new SelectListItem { Value = ((int)s).ToString(), Text = s.ToString() })
            .ToList();
    }
} 
