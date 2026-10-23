using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using BookIt.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookIt.Tests;

/// <summary>
/// Proves the mechanism the service relies on: the rowversion column makes a
/// stale UPDATE match zero rows, and EF turns that into DbUpdateConcurrencyException.
/// </summary>
[Collection(DatabaseCollection.Name)]
public class ConcurrencyTokenTests
{
    private readonly SqlServerFixture _db;

    public ConcurrencyTokenTests(SqlServerFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task StaleRowVersion_SecondWriterGetsDbUpdateConcurrencyException()
    {
        var seatId = _db.SeatIds[9];

        using var scope1 = _db.NewScope();
        using var scope2 = _db.NewScope();
        var db1 = scope1.ServiceProvider.GetRequiredService<AppDbContext>();
        var db2 = scope2.ServiceProvider.GetRequiredService<AppDbContext>();

        // both "requests" read the same row (and the same rowversion) before either writes
        var row1 = await db1.EventSeats.SingleAsync(es => es.EventId == _db.EventId && es.SeatId == seatId);
        var row2 = await db2.EventSeats.SingleAsync(es => es.EventId == _db.EventId && es.SeatId == seatId);
        Assert.Equal(row1.RowVersion, row2.RowVersion);

        row1.Status = EventSeatStatus.Held;
        row1.HeldByUserId = _db.UserA;
        row1.HeldUntil = DateTime.UtcNow.AddMinutes(10);
        await db1.SaveChangesAsync();

        row2.Status = EventSeatStatus.Held;
        row2.HeldByUserId = _db.UserB;
        row2.HeldUntil = DateTime.UtcNow.AddMinutes(10);
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => db2.SaveChangesAsync());

        using var verify = _db.NewScope();
        var fresh = await verify.ServiceProvider.GetRequiredService<AppDbContext>().EventSeats
            .AsNoTracking().SingleAsync(es => es.EventId == _db.EventId && es.SeatId == seatId);
        Assert.Equal(_db.UserA, fresh.HeldByUserId);
        Assert.NotEqual(row1.RowVersion.AsEnumerable(), row2.RowVersion.AsEnumerable());
    }

    [Fact]
    public async Task UniqueIndex_RejectsASecondRowForTheSameEventAndSeat()
    {
        using var scope = _db.NewScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.EventSeats.Add(new Domain.Entities.EventSeat { EventId = _db.EventId, SeatId = _db.SeatIds[0] });

        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }
}
