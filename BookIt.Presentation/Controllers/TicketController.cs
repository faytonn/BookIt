using BookIt.Application.Interfaces.Services;
using BookIt.Application.Interfaces.Services.External;
using BookIt.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Controllers;

[Authorize]
public class TicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IQrCodeService _qrCodeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketController(ITicketService ticketService, IQrCodeService qrCodeService, UserManager<ApplicationUser> userManager)
    {
        _ticketService = ticketService;
        _qrCodeService = qrCodeService;
        _userManager = userManager;
    }

    [HttpGet("/tickets/{code}")]
    public async Task<IActionResult> Index(string code)
    {
        var booking = await _ticketService.GetBookingAsync(code);
        // someone else's booking looks exactly like a missing one, so booking codes can't be probed
        if (booking == null || !_canSee(booking.UserId))
            return NotFound();

        return View(booking);
    }

    [HttpGet("/tickets/qr/{ticketCode}")]
    public async Task<IActionResult> Qr(string ticketCode)
    {
        var owner = await _ticketService.GetTicketOwnerAsync(ticketCode);
        if (owner == null || !_canSee(owner))
            return NotFound();

        // the QR is the ticket: keep it out of shared caches
        Response.Headers.CacheControl = "private, no-store";
        return File(_qrCodeService.Png(ticketCode.Trim().ToUpperInvariant()), "image/png");
    }

    private bool _canSee(string ownerId) =>
        ownerId == _userManager.GetUserId(User) || User.IsInRole("Admin") || User.IsInRole("Moderator");
}
