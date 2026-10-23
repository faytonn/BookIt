using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Persistence.DataInitializers;

/// <summary>
/// Creates one big event (5,000 seats) and a pool of confirmed users for load testing.
/// Idempotent: re-running finds the existing rows and only fills in what is missing.
/// Run with: dotnet run --project BookIt.Presentation -- seed-loadtest
/// </summary>
public class LoadTestSeeder
{
    public const string EventTitle = "On-Sale Load Test";
    public const string UserPassword = "LoadTest123!";
    public const int Rows = 100;
    public const int Columns = 50;
    public const int UserCount = 100;

    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEventSeatService _eventSeatService;
    private readonly ILogger<LoadTestSeeder> _logger;

    public LoadTestSeeder(AppDbContext db, UserManager<ApplicationUser> userManager, IEventSeatService eventSeatService, ILogger<LoadTestSeeder> logger)
    {
        _db = db;
        _userManager = userManager;
        _eventSeatService = eventSeatService;
        _logger = logger;
    }

    public async Task<int> SeedAsync()
    {
        var ev = await _db.Events.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Title == EventTitle);
        if (ev == null)
        {
            ev = await _createVenueAndEventAsync();
            _logger.LogInformation("Created load-test event {EventId} with {Seats} seats", ev.Id, Rows * Columns);
        }
        else
        {
            _logger.LogInformation("Load-test event {EventId} already exists", ev.Id);
        }

        var created = await _eventSeatService.EnsureSeatsForEventAsync(ev.Id);
        _logger.LogInformation("EventSeat rows created: {Count}", created);

        await _createUsersAsync();

        return ev.Id;
    }

    private async Task<Event> _createVenueAndEventAsync()
    {
        var location = new GeneralLocation { Name = "Load Test Arena", Address = "Výstaviště 67", City = "Prague", Country = "Czech Republic" };
        var hall = new Hall { Name = "Big Hall", Location = location };
        var seatType = new SeatType { Name = "Standard", DefaultPrice = 29.90m, Hall = hall };
        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Name == "Concert")
                       ?? new Category { Name = "Concert" };

        var seats = new List<Seat>(Rows * Columns);
        for (var row = 1; row <= Rows; row++)
        {
            for (var col = 1; col <= Columns; col++)
            {
                seats.Add(new Seat { SeatName = $"R{row}-{col}", SeatRow = row, SeatColumn = col, SeatType = seatType, Hall = hall });
            }
        }

        var ev = new Event
        {
            Title = EventTitle,
            ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738786345/bookit./KendrickLamar.webp",
            EventDate = DateTime.UtcNow.AddDays(30),
            PriceRange = "29.90-29.90",
            Category = category,
            GeneralLocation = location,
            Hall = hall
        };

        _db.AddRange(location, hall, seatType, ev);
        _db.AddRange(seats);
        await _db.SaveChangesAsync();
        return ev;
    }

    private async Task _createUsersAsync()
    {
        var created = 0;
        for (var i = 1; i <= UserCount; i++)
        {
            var email = $"loadtest-{i}@bookit.test";
            if (await _userManager.FindByEmailAsync(email) != null)
                continue;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "Load",
                LastName = $"Tester {i}",
                IsActive = true,
                LockoutEnabled = false
            };

            var result = await _userManager.CreateAsync(user, UserPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Could not create {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await _userManager.AddToRoleAsync(user, Role.User.ToString());
            created++;
        }
        _logger.LogInformation("Load-test users created: {Created} (pool size {Total})", created, UserCount);
    }
}
