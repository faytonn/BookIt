using BookIt.Application.Interfaces.Services;
using BookIt.Application.ServiceRegistrations;
using BookIt.Domain.Entities;
using BookIt.Infrastracture;
using BookIt.Persistence.Contexts;
using BookIt.Persistence.ServiceRegistrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace BookIt.Tests.Fixtures;

/// <summary>
/// One real SQL Server in Docker for the whole test collection, migrated with the
/// production migrations and seeded with a small venue. The in-memory provider is
/// not used anywhere: it ignores rowversion tokens and unique indexes, which are
/// exactly what these tests exist to prove.
/// </summary>
public sealed class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public ServiceProvider Services { get; private set; } = null!;

    public int EventId { get; private set; }
    public int HallId { get; private set; }
    public List<int> SeatIds { get; } = [];
    public string UserA { get; } = Guid.NewGuid().ToString();
    public string UserB { get; } = Guid.NewGuid().ToString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString()
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);
        services.AddInfrastractureServices(configuration);
        Services = services.BuildServiceProvider();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
        await _seedAsync(db);

        var eventSeatService = scope.ServiceProvider.GetRequiredService<IEventSeatService>();
        await eventSeatService.EnsureSeatsForEventAsync(EventId);
    }

    public async Task DisposeAsync()
    {
        await Services.DisposeAsync();
        await _container.DisposeAsync();
    }

    /// <summary>Fresh DI scope = fresh DbContext, i.e. one simulated request.</summary>
    public IServiceScope NewScope() => Services.CreateScope();

    private async Task _seedAsync(AppDbContext db)
    {
        db.Users.AddRange(
            new ApplicationUser { Id = UserA, UserName = "a@test.local", Email = "a@test.local", FirstName = "A", LastName = "Test", EmailConfirmed = true },
            new ApplicationUser { Id = UserB, UserName = "b@test.local", Email = "b@test.local", FirstName = "B", LastName = "Test", EmailConfirmed = true });

        var location = new GeneralLocation { Name = "Test Arena", Address = "Street 1", City = "Prague", Country = "CZ" };
        var hall = new Hall { Name = "Main Hall", Location = location };
        var seatType = new SeatType { Name = "Standard", DefaultPrice = 25m, Hall = hall };
        var category = new Category { Name = "Concert" };

        var seats = Enumerable.Range(1, 10)
            .Select(i => new Seat { SeatName = $"A{i}", SeatRow = 1, SeatColumn = i, SeatType = seatType, Hall = hall })
            .ToList();

        var ev = new Event
        {
            Title = "Load Test Show",
            ImagePath = "https://example.com/x.jpg",
            EventDate = DateTime.UtcNow.AddDays(30),
            PriceRange = "25-25",
            Category = category,
            GeneralLocation = location,
            Hall = hall
        };

        db.AddRange(location, hall, seatType, category, ev);
        db.AddRange(seats);
        await db.SaveChangesAsync();

        EventId = ev.Id;
        HallId = hall.Id;
        SeatIds.AddRange(seats.Select(s => s.Id));
    }
}

[CollectionDefinition(Name)]
public class DatabaseCollection : ICollectionFixture<SqlServerFixture>
{
    public const string Name = "database";
}
