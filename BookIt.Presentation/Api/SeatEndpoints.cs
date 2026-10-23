using System.Security.Claims;
using BookIt.Application.DTOs.EventSeatDTO;
using BookIt.Application.Exceptions;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Api;

/// <summary>
/// Minimal JSON API for the booking flow. Sits next to MVC and reuses the same
/// cookie authentication; unauthenticated calls get 401/403 instead of a redirect.
/// </summary>
public static class SeatEndpoints
{
    public record HoldRequest(int SeatId);

    public static IEndpointRouteBuilder MapSeatApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api").WithTags("Seats");

        api.MapGet("/events/{eventId:int}/seats", GetSeatMap)
           .WithName("GetSeatMap");

        api.MapPost("/events/{eventId:int}/holds", HoldSeat)
           .WithName("HoldSeat")
           .RequireAuthorization()
           .RequireRateLimiting(HoldRateLimiting.PolicyName);

        api.MapPost("/holds/{holdId:int}/confirm", ConfirmHold)
           .WithName("ConfirmHold")
           .RequireAuthorization();

        api.MapDelete("/holds/{holdId:int}", ReleaseHold)
           .WithName("ReleaseHold")
           .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetSeatMap(int eventId, ClaimsPrincipal user, IEventSeatService eventSeatService)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        List<GetEventSeatDTO> seats;
        try
        {
            seats = await eventSeatService.GetSeatMapAsync(eventId, userId);
        }
        catch (NotFoundException)
        {
            // the MVC NotFoundExceptionFilter does not cover minimal API endpoints
            return Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Event not found.");
        }

        return Results.Ok(seats.Select(s => new
        {
            eventSeatId = s.Id,
            seatId = s.SeatId,
            name = s.SeatName,
            row = s.Row,
            column = s.Column,
            seatType = s.SeatTypeName,
            price = s.Price,
            status = s.Status.ToString(),
            heldUntil = s.HeldUntil,
            heldByMe = s.HeldByMe
        }));
    }

    private static async Task<IResult> HoldSeat(int eventId, [FromBody] HoldRequest request, ClaimsPrincipal user, IEventSeatService eventSeatService)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await eventSeatService.HoldAsync(eventId, request.SeatId, userId);

        return result.Outcome switch
        {
            HoldOutcome.Success => Results.Created($"/api/holds/{result.EventSeatId}", new
            {
                holdId = result.EventSeatId,
                eventId,
                seatId = request.SeatId,
                heldUntil = result.HeldUntil
            }),
            HoldOutcome.NotFound => Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Seat not found for this event."),
            _ => Results.Problem(statusCode: StatusCodes.Status409Conflict, title: "Seat is not available.")
        };
    }

    private static async Task<IResult> ConfirmHold(int holdId, ClaimsPrincipal user, IEventSeatService eventSeatService)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await eventSeatService.ConfirmAsync(holdId, userId);

        return result.Outcome switch
        {
            HoldOutcome.Success => Results.Ok(new { holdId, reservationId = result.ReservationId }),
            HoldOutcome.NotFound => Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Hold not found."),
            HoldOutcome.HoldExpired => Results.Problem(statusCode: StatusCodes.Status410Gone, title: "Your hold has expired or belongs to someone else."),
            _ => Results.Problem(statusCode: StatusCodes.Status409Conflict, title: "Seat is already sold.")
        };
    }

    private static async Task<IResult> ReleaseHold(int holdId, ClaimsPrincipal user, IEventSeatService eventSeatService)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await eventSeatService.ReleaseAsync(holdId, userId);

        return result.Outcome switch
        {
            HoldOutcome.Success => Results.NoContent(),
            HoldOutcome.NotFound => Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Hold not found."),
            HoldOutcome.HoldExpired => Results.Problem(statusCode: StatusCodes.Status410Gone, title: "No active hold of yours on this seat."),
            _ => Results.Problem(statusCode: StatusCodes.Status409Conflict, title: "Seat changed meanwhile.")
        };
    }
}
