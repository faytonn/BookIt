using BookIt.Application.DTOs.EventSeatDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using BookIt.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookIt.Tests;

[Collection(DatabaseCollection.Name)]
public class EventSeatServiceTests
{
    private readonly SqlServerFixture _db;

    public EventSeatServiceTests(SqlServerFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task TwoParallelHolds_OnTheSameSeat_ExactlyOneSucceeds()
    {
        var seatId = _db.SeatIds[0];

        // Repeat to give the race a real chance to interleave; every round must end 1/1.
        for (var round = 0; round < 10; round++)
        {
            await _resetSeatAsync(seatId);

            var a = Task.Run(() => _holdAsync(seatId, _db.UserA));
            var b = Task.Run(() => _holdAsync(seatId, _db.UserB));
            var results = await Task.WhenAll(a, b);

            Assert.Equal(1, results.Count(r => r.Outcome == HoldOutcome.Success));
            Assert.Equal(1, results.Count(r => r.Outcome == HoldOutcome.Conflict));

            var row = await _loadAsync(seatId);
            Assert.Equal(EventSeatStatus.Held, row.Status);
            Assert.Contains(row.HeldByUserId, new[] { _db.UserA, _db.UserB });
        }
    }

    [Fact]
    public async Task ManyParallelHolds_OnTheSameSeat_ExactlyOneSucceeds()
    {
        var seatId = _db.SeatIds[1];
        await _resetSeatAsync(seatId);

        var users = Enumerable.Range(0, 20).Select(_ => Guid.NewGuid().ToString()).ToList();
        await _createUsersAsync(users);

        var results = await Task.WhenAll(users.Select(u => Task.Run(() => _holdAsync(seatId, u))));

        Assert.Equal(1, results.Count(r => r.Outcome == HoldOutcome.Success));
        Assert.Equal(19, results.Count(r => r.Outcome == HoldOutcome.Conflict));
    }

    [Fact]
    public async Task Hold_ThenConfirm_SellsTheSeatAndCreatesAReservation()
    {
        var seatId = _db.SeatIds[2];
        await _resetSeatAsync(seatId);

        var hold = await _holdAsync(seatId, _db.UserA);
        Assert.Equal(HoldOutcome.Success, hold.Outcome);
        Assert.NotNull(hold.HeldUntil);
        Assert.InRange(hold.HeldUntil!.Value, DateTime.UtcNow.AddMinutes(9), DateTime.UtcNow.AddMinutes(11));

        HoldResultDTO confirm;
        using (var scope = _db.NewScope())
        {
            confirm = await scope.ServiceProvider.GetRequiredService<IEventSeatService>().ConfirmAsync(hold.EventSeatId!.Value, _db.UserA);
        }
        Assert.Equal(HoldOutcome.Success, confirm.Outcome);
        Assert.NotNull(confirm.ReservationId);

        using var verify = _db.NewScope();
        var db = verify.ServiceProvider.GetRequiredService<AppDbContext>();
        var row = await db.EventSeats.SingleAsync(es => es.EventId == _db.EventId && es.SeatId == seatId);
        Assert.Equal(EventSeatStatus.Sold, row.Status);
        Assert.Null(row.HeldUntil);
        Assert.Equal(confirm.ReservationId, row.ReservationId);

        var reservation = await db.Reservations.Include(r => r.ReservationSeats).SingleAsync(r => r.Id == confirm.ReservationId);
        Assert.Equal(ReservationStatus.Confirmed, reservation.Status);
        Assert.Equal(_db.UserA, reservation.UserId);
        Assert.Equal(25m, reservation.TotalAmount);
        Assert.Single(reservation.ReservationSeats!);
        Assert.Equal(seatId, reservation.ReservationSeats!.Single().SeatId);
    }

    [Fact]
    public async Task Confirm_ByAnotherUser_IsRejected()
    {
        var seatId = _db.SeatIds[3];
        await _resetSeatAsync(seatId);

        var hold = await _holdAsync(seatId, _db.UserA);
        Assert.Equal(HoldOutcome.Success, hold.Outcome);

        using var scope = _db.NewScope();
        var result = await scope.ServiceProvider.GetRequiredService<IEventSeatService>().ConfirmAsync(hold.EventSeatId!.Value, _db.UserB);

        Assert.Equal(HoldOutcome.HoldExpired, result.Outcome);
        Assert.Equal(EventSeatStatus.Held, (await _loadAsync(seatId)).Status);
    }

    [Fact]
    public async Task ExpiredHold_CountsAsAvailable_AndCanBeTakenBySomeoneElse()
    {
        var seatId = _db.SeatIds[4];
        await _resetSeatAsync(seatId);

        var first = await _holdAsync(seatId, _db.UserA);
        Assert.Equal(HoldOutcome.Success, first.Outcome);
        await _expireHoldAsync(seatId);

        using (var scope = _db.NewScope())
        {
            var map = await scope.ServiceProvider.GetRequiredService<IEventSeatService>().GetSeatMapAsync(_db.EventId, _db.UserB);
            Assert.Equal(EventSeatStatus.Available, map.Single(s => s.SeatId == seatId).Status);
        }

        var second = await _holdAsync(seatId, _db.UserB);
        Assert.Equal(HoldOutcome.Success, second.Outcome);
        Assert.Equal(_db.UserB, (await _loadAsync(seatId)).HeldByUserId);

        // the original holder can no longer confirm
        using var confirmScope = _db.NewScope();
        var stale = await confirmScope.ServiceProvider.GetRequiredService<IEventSeatService>().ConfirmAsync(first.EventSeatId!.Value, _db.UserA);
        Assert.Equal(HoldOutcome.HoldExpired, stale.Outcome);
    }

    [Fact]
    public async Task Hold_OnSoldSeat_IsConflict()
    {
        var seatId = _db.SeatIds[5];
        await _resetSeatAsync(seatId);

        var hold = await _holdAsync(seatId, _db.UserA);
        using (var scope = _db.NewScope())
        {
            await scope.ServiceProvider.GetRequiredService<IEventSeatService>().ConfirmAsync(hold.EventSeatId!.Value, _db.UserA);
        }

        var result = await _holdAsync(seatId, _db.UserB);
        Assert.Equal(HoldOutcome.Conflict, result.Outcome);
    }

    [Fact]
    public async Task ReleaseExpiredHolds_FreesOnlyExpiredOnes()
    {
        var expiredSeat = _db.SeatIds[6];
        var activeSeat = _db.SeatIds[7];
        await _resetSeatAsync(expiredSeat);
        await _resetSeatAsync(activeSeat);

        await _holdAsync(expiredSeat, _db.UserA);
        await _holdAsync(activeSeat, _db.UserA);
        await _expireHoldAsync(expiredSeat);

        int released;
        using (var scope = _db.NewScope())
        {
            released = await scope.ServiceProvider.GetRequiredService<IEventSeatService>().ReleaseExpiredHoldsAsync();
        }

        Assert.True(released >= 1);
        Assert.Equal(EventSeatStatus.Available, (await _loadAsync(expiredSeat)).Status);
        Assert.Equal(EventSeatStatus.Held, (await _loadAsync(activeSeat)).Status);
    }

    [Fact]
    public async Task Release_ByHolder_FreesTheSeat()
    {
        var seatId = _db.SeatIds[8];
        await _resetSeatAsync(seatId);

        var hold = await _holdAsync(seatId, _db.UserA);

        using var scope = _db.NewScope();
        var result = await scope.ServiceProvider.GetRequiredService<IEventSeatService>().ReleaseAsync(hold.EventSeatId!.Value, _db.UserA);

        Assert.Equal(HoldOutcome.Success, result.Outcome);
        var row = await _loadAsync(seatId);
        Assert.Equal(EventSeatStatus.Available, row.Status);
        Assert.Null(row.HeldByUserId);
    }

    // ---- helpers: every call gets its own scope, i.e. its own DbContext, like a real request ----

    private async Task<HoldResultDTO> _holdAsync(int seatId, string userId)
    {
        using var scope = _db.NewScope();
        return await scope.ServiceProvider.GetRequiredService<IEventSeatService>().HoldAsync(_db.EventId, seatId, userId);
    }

    private async Task<EventSeat> _loadAsync(int seatId)
    {
        using var scope = _db.NewScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await db.EventSeats.AsNoTracking().SingleAsync(es => es.EventId == _db.EventId && es.SeatId == seatId);
    }

    private async Task _resetSeatAsync(int seatId)
    {
        using var scope = _db.NewScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.EventSeats
            .Where(es => es.EventId == _db.EventId && es.SeatId == seatId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(es => es.Status, EventSeatStatus.Available)
                .SetProperty(es => es.HeldUntil, (DateTime?)null)
                .SetProperty(es => es.HeldByUserId, (string?)null)
                .SetProperty(es => es.ReservationId, (int?)null));
    }

    private async Task _expireHoldAsync(int seatId)
    {
        using var scope = _db.NewScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.EventSeats
            .Where(es => es.EventId == _db.EventId && es.SeatId == seatId)
            .ExecuteUpdateAsync(s => s.SetProperty(es => es.HeldUntil, DateTime.UtcNow.AddMinutes(-1)));
    }

    private async Task _createUsersAsync(IEnumerable<string> ids)
    {
        using var scope = _db.NewScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        foreach (var id in ids)
        {
            db.Users.Add(new ApplicationUser { Id = id, UserName = $"{id}@test.local", Email = $"{id}@test.local", FirstName = "U", LastName = "Test", EmailConfirmed = true });
        }
        await db.SaveChangesAsync();
    }
}
