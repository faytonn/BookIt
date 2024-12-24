using Bogus;
using BookIt.Core.Domain.Entities;
using BookIt.Infrastracture.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace BookIt.Infrastracture.Persistence.Seeder
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Clear existing data
            var existingEvents = await _context.Events.Where(e => !e.IsDeleted).ToListAsync();
            foreach (var evt in existingEvents)
            {
                evt.IsDeleted = true;
            }

            var existingUsers = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            foreach (var user in existingUsers)
            {
                user.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            // Create users
            var userFaker = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.PasswordHash, f => BC.HashPassword("Password123!"))
                .RuleFor(u => u.Role, f => f.Random.ArrayElement(new[] { "User", "Organizer" }))
                .RuleFor(u => u.ProfileImage, f => f.Internet.Avatar())
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(1))
                .RuleFor(u => u.UpdatedAt, (f, u) => u.CreatedAt)
                .RuleFor(u => u.IsDeleted, false);

            var users = userFaker.Generate(50);
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            // Get organizers for events
            var organizers = users.Where(u => u.Role == "Organizer").ToList();

            // Create events
            var eventFaker = new Faker<Event>()
                .RuleFor(e => e.Title, f => f.Commerce.ProductName())
                .RuleFor(e => e.Description, f => f.Lorem.Paragraphs(2))
                .RuleFor(e => e.Type, f => f.Random.ArrayElement(new[] { "conference", "exhibition", "sport", "concert", "theater", "other" }))
                .RuleFor(e => e.LocationName, f => f.Company.CompanyName())
                .RuleFor(e => e.LocationAddress, f => f.Address.StreetAddress())
                .RuleFor(e => e.LocationCity, f => f.Address.City())
                .RuleFor(e => e.LocationCountry, f => f.Address.Country())
                .RuleFor(e => e.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(e => e.StartDate, f => f.Date.Future(1))
                .RuleFor(e => e.EndDate, (f, e) => e.StartDate.AddDays(f.Random.Number(1, 5)))
                .RuleFor(e => e.Tags, f => new List<string> { f.Commerce.Department(), f.Commerce.Department(), f.Commerce.Department() })
                .RuleFor(e => e.OrganizerId, f => f.PickRandom(organizers).Id)
                .RuleFor(e => e.CreatedAt, f => f.Date.Past(1))
                .RuleFor(e => e.UpdatedAt, (f, e) => e.CreatedAt)
                .RuleFor(e => e.IsDeleted, false);

            var events = eventFaker.Generate(50);

            // Create event pricing for each event
            var pricingFaker = new Faker<EventPricing>()
                .RuleFor(p => p.Category, f => f.Random.ArrayElement(new[] { "Standard", "VIP", "Premium", "Early Bird" }))
                .RuleFor(p => p.Price, f => f.Random.Decimal(50, 1000))
                .RuleFor(p => p.TotalSeats, f => f.Random.Number(50, 200))
                .RuleFor(p => p.AvailableSeats, (f, p) => p.TotalSeats)
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
                .RuleFor(p => p.UpdatedAt, (f, p) => p.CreatedAt);

            foreach (var evt in events)
            {
                var numberOfPricingCategories = new Random().Next(1, 4);
                evt.Pricing = pricingFaker.Generate(numberOfPricingCategories);
            }

            await _context.Events.AddRangeAsync(events);
            await _context.SaveChangesAsync();
        }
    }
}
