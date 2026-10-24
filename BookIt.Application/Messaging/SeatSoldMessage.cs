namespace BookIt.Application.Messaging;

/// <summary>
/// Published once per confirmed seat. Carries ids plus the few display values the
/// consumer needs, so it can work without re-querying everything.
/// </summary>
public record SeatSoldMessage(
    int ReservationId,
    int EventId,
    string EventTitle,
    DateTime EventDate,
    int SeatId,
    string SeatName,
    decimal Price,
    string UserId,
    DateTime SoldAtUtc)
{
    public const string RoutingKey = "seat.sold";
}
