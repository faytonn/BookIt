using System.Text.Json;
using System.Text.Json.Serialization;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Areas.Admin.Controllers;

/// <summary>Door check-in. Moderators are door staff: this is the only admin page they can open.</summary>
[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class CheckInController : Controller
{
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly ITicketService _ticketService;

    public CheckInController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index(int? eventId)
    {
        var events = await _ticketService.GetCheckInEventsAsync();
        ViewBag.Events = events;
        ViewBag.SelectedEvent = events.FirstOrDefault(e => e.EventId == eventId);
        return View();
    }

    // called by the scanner page with fetch(); answers JSON so the next scan can follow right away
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Scan(int eventId, string? code)
    {
        var result = await _ticketService.CheckInAsync(code ?? string.Empty, eventId, User.Identity?.Name ?? "staff");
        return Json(result, _json);
    }
}
