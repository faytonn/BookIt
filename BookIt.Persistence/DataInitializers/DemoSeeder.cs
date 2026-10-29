using System.Globalization;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Persistence.DataInitializers;

/// <summary>
/// Fills the site with demo content: 8 categories, 5 venues with seated halls, 30 events
/// (texts in en/az/cs), confirmed sales from the load-test users and 6 news posts.
/// Posters live in wwwroot/user/assets/images/demo.
/// Idempotent: rows are matched by name/title, so re-running only adds what is missing.
/// Run with: dotnet run --project BookIt.Presentation -- seed-demo
/// </summary>
public class DemoSeeder
{
    private const string _imageRoot = "/user/assets/images/demo";

    private readonly AppDbContext _db;
    private readonly IEventSeatService _eventSeatService;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<DemoSeeder> _logger;

    public DemoSeeder(AppDbContext db, IEventSeatService eventSeatService, IEventRepository eventRepository, ILogger<DemoSeeder> logger)
    {
        _db = db;
        _eventSeatService = eventSeatService;
        _eventRepository = eventRepository;
        _logger = logger;
    }

    private record CategorySeed(string Key, string En, string Az, string Cs, string DescEn, string DescAz, string DescCs);
    private record HallSeed(string Name, int Rows, int Columns, decimal BasePrice);
    private record VenueSeed(string Key, string Name, string Address, string City, string Country, HallSeed[] Halls);
    private record EventSeed(string Slug, string Title, string Category, string Venue, int Hall, int DaysFromToday, int Hour);
    private record NewsSeed(string Slug, string En, string Az, string Cs, string BodyEn, string BodyAz, string BodyCs);

    // {0} = event title, {1} = venue, {2} = city
    private static readonly CategorySeed[] _categories =
    [
        new("concert", "Concert", "Konsert", "Koncert",
            "{0} brings a full live show to {1}, {2}. Expect a big sound, a light show and a crowd that sings along. Doors open one hour before the start.",
            "{0} {1} ({2}) səhnəsinə tam canlı şou gətirir. Güclü səs, işıq şousu və birlikdə oxuyan izləyicilər sizi gözləyir. Qapılar başlamadan bir saat əvvəl açılır.",
            "{0} přiváží velkou živou show do {1} ({2}). Čeká vás silný zvuk, světelná show a publikum, které zpívá s kapelou. Vstup se otevírá hodinu před začátkem."),
        new("theatre", "Theatre", "Teatr", "Divadlo",
            "{0} at {1}, {2}. An evening of live theatre in an intimate hall, with one interval. Recommended for ages 12 and up.",
            "{0} — {1} ({2}). Bir fasilə ilə kamera zalında canlı teatr axşamı. 12 yaşdan yuxarı tamaşaçılar üçün tövsiyə olunur.",
            "{0} – {1} ({2}). Večer živého divadla v komorním sále s jednou přestávkou. Doporučeno od 12 let."),
        new("comedy", "Stand-up Comedy", "Stand-up komediya", "Stand-up komedie",
            "{0} at {1}, {2}. Two hours of stand-up from local and touring comedians. Performed in English. Ages 16+.",
            "{0} — {1} ({2}). Yerli və qonaq komediyaçılardan iki saatlıq stand-up. İngilis dilində. 16 yaşdan yuxarı.",
            "{0} – {1} ({2}). Dvě hodiny stand-upu od místních i hostujících komiků. V angličtině. Od 16 let."),
        new("sports", "Sports", "İdman", "Sport",
            "{0} at {1}, {2}. Fast games, loud fans and a great view from every seat. Arrive early: security checks take time on match days.",
            "{0} — {1} ({2}). Sürətli oyun, səs-küylü azarkeşlər və hər yerdən gözəl mənzərə. Erkən gəlin: oyun günü təhlükəsizlik yoxlaması vaxt aparır.",
            "{0} – {1} ({2}). Rychlá hra, hlasití fanoušci a skvělý výhled z každého místa. Přijďte včas: bezpečnostní kontroly v den zápasu chvíli trvají."),
        new("festival", "Festival", "Festival", "Festival",
            "{0} fills {1}, {2} with music, food stalls and workshops all day. One ticket covers every stage.",
            "{0} bütün gün {1} ({2}) məkanını musiqi, yemək köşkləri və master-klasslarla doldurur. Bir bilet bütün səhnələr üçün keçərlidir.",
            "{0} zaplní {1} ({2}) hudbou, stánky s jídlem a workshopy po celý den. Jedna vstupenka platí na všechna pódia."),
        new("kids", "Kids & Family", "Uşaqlar və ailə", "Děti a rodina",
            "{0} at {1}, {2}. A colourful one-hour show for children aged 4 to 10 and their families. Kids under 3 sit on a parent's lap for free.",
            "{0} — {1} ({2}). 4–10 yaşlı uşaqlar və ailələri üçün bir saatlıq rəngarəng şou. 3 yaşadək uşaqlar valideynin qucağında pulsuz oturur.",
            "{0} – {1} ({2}). Barevná hodinová show pro děti od 4 do 10 let a jejich rodiny. Děti do 3 let sedí rodičům na klíně zdarma."),
        new("classical", "Opera & Classical", "Opera və klassik musiqi", "Opera a klasická hudba",
            "{0}, performed live at {1}, {2}. Live musicians and soloists in one of the city's best acoustic halls. Please arrive 15 minutes early; late entry only at the interval.",
            "{0} — {1} ({2}) səhnəsində canlı ifa. Şəhərin ən yaxşı akustik zallarından birində canlı musiqiçilər və solistlər. Zəhmət olmasa 15 dəqiqə tez gəlin; gecikənlər yalnız fasilədə içəri buraxılır.",
            "{0} živě v {1} ({2}). Živí hudebníci a sólisté v jednom z akusticky nejlepších sálů ve městě. Přijďte prosím 15 minut předem; pozdní vstup jen o přestávce."),
        new("exhibition", "Exhibition", "Sərgi", "Výstava",
            "{0} at {1}, {2}. Walk through at your own pace; most visitors spend about 90 minutes. Your ticket is valid for one entry on the chosen day.",
            "{0} — {1} ({2}). Öz tempinizdə gəzin; ziyarətçilərin çoxu təxminən 90 dəqiqə vaxt keçirir. Bilet seçilmiş gündə bir giriş üçün keçərlidir.",
            "{0} – {1} ({2}). Prohlídka vlastním tempem; většina návštěvníků stráví asi 90 minut. Vstupenka platí na jeden vstup ve vybraný den."),
    ];

    private static readonly VenueSeed[] _venues =
    [
        new("v1", "Vltava Arena", "Ke Stadionu 12", "Prague", "Czech Republic", [new("Main Arena", 12, 20, 39m), new("Club Stage", 6, 10, 19m)]),
        new("v2", "Old Town Stage", "Karlova 21", "Prague", "Czech Republic", [new("Grand Hall", 8, 14, 29m)]),
        new("v3", "Riverside Hall", "Nábřeží Svratky 5", "Brno", "Czech Republic", [new("Riverside Hall", 10, 16, 25m)]),
        new("v4", "Silesia Event Centre", "Hornická 40", "Ostrava", "Czech Republic", [new("Hall A", 10, 18, 22m)]),
        new("v5", "Caspian Concert Hall", "Neftçilər prospekti 88", "Baku", "Azerbaijan", [new("Main Hall", 9, 15, 30m)]),
    ];

    private static readonly EventSeed[] _events =
    [
        new("neon-harbour-live", "Neon Harbour — Live", "concert", "v1", 0, 5, 20),
        new("midnight-static-tour", "The Midnight Static Tour", "concert", "v1", 0, 19, 20),
        new("jazz-under-the-bridges", "Jazz Under the Bridges", "concert", "v2", 0, 9, 19),
        new("paper-lanterns-indie-night", "Indie Night: Paper Lanterns", "concert", "v1", 1, 3, 21),
        new("synthwave-sunset", "Synthwave Sunset", "concert", "v3", 0, -14, 20),
        new("baku-jazz-nights", "Baku Jazz Nights", "concert", "v5", 0, 26, 20),
        new("hamlet-modern-staging", "Hamlet — A Modern Staging", "theatre", "v2", 0, 12, 19),
        new("the-last-tram-home", "The Last Tram Home", "theatre", "v2", 0, 33, 19),
        new("twelfth-night", "Twelfth Night", "theatre", "v3", 0, 40, 19),
        new("a-room-full-of-clocks", "A Room Full of Clocks", "theatre", "v4", 0, 22, 19),
        new("lost-in-translation-standup", "Lost in Translation: Stand-up Night", "comedy", "v1", 1, 7, 20),
        new("open-mic-friday", "Open Mic Friday", "comedy", "v1", 1, 14, 21),
        new("bad-decisions-comedy", "Comedy Night: Bad Decisions", "comedy", "v4", 0, 28, 20),
        new("basketball-city-cup-final", "City Basketball Cup Final", "sports", "v1", 0, 16, 18),
        new("futsal-champions-weekend", "Futsal Champions Weekend", "sports", "v4", 0, 35, 15),
        new("winter-classic-hockey", "Winter Classic: Exhibition Hockey", "sports", "v1", 0, 60, 17),
        new("lantern-light-festival", "Lantern Light Festival", "festival", "v3", 0, 45, 18),
        new("street-food-and-music", "Street Food & Music Festival", "festival", "v4", 0, 11, 16),
        new("autumn-folk-festival", "Autumn Folk Festival", "festival", "v2", 0, 4, 17),
        new("novruz-spring-festival", "Novruz Spring Celebration", "festival", "v5", 0, 140, 17),
        new("puppet-circus", "The Puppet Circus", "kids", "v2", 0, 6, 11),
        new("science-magic-show", "Science Magic Show", "kids", "v3", 0, 13, 11),
        new("dinosaur-adventure-live", "Dinosaur Adventure Live", "kids", "v4", 0, 20, 10),
        new("magic-flute", "The Magic Flute", "classical", "v2", 0, 30, 19),
        new("new-world-symphony", "Dvořák: New World Symphony", "classical", "v3", 0, 24, 19),
        new("four-seasons-candlelight", "Vivaldi: The Four Seasons by Candlelight", "classical", "v2", 0, 17, 20),
        new("mugham-evening", "Mugham Evening", "classical", "v5", 0, 10, 19),
        new("robots-and-us", "Robots & Us: Interactive Exhibition", "exhibition", "v4", 0, 2, 10),
        new("light-and-shadow-photo", "Light & Shadow: Photo Exhibition", "exhibition", "v3", 0, 8, 10),
        new("old-maps-of-europe", "Old Maps of Europe", "exhibition", "v2", 0, -5, 10),
    ];

    private static readonly NewsSeed[] _news =
    [
        new("winter-season-lineup", "Winter season line-up announced", "Qış mövsümünün proqramı açıqlandı", "Program zimní sezóny je venku",
            "Thirty shows across five venues, from jazz nights to a candlelit Vivaldi. Tickets for every date are on sale now.",
            "Beş məkanda otuz tədbir: caz axşamlarından şam işığında Vivaldiyə qədər. Bütün tarixlər üçün biletlər artıq satışdadır.",
            "Třicet představení v pěti sálech, od jazzových večerů po Vivaldiho při svíčkách. Vstupenky na všechny termíny jsou v prodeji."),
        new("new-venue-ostrava", "Silesia Event Centre joins BookIt", "Silesia Event Centre BookIt-ə qoşuldu", "Silesia Event Centre je nově na BookIt",
            "Ostrava's Silesia Event Centre now sells its seats through BookIt, starting with comedy, futsal and a robot exhibition.",
            "Ostravadakı Silesia Event Centre artıq yerlərini BookIt vasitəsilə satır: komediya, futzal və robot sərgisi ilə başlayır.",
            "Ostravské Silesia Event Centre nyní prodává místa přes BookIt, začíná komedií, futsalem a výstavou robotů."),
        new("how-seat-holds-work", "How seat holds work", "Yer rezervi necə işləyir", "Jak funguje držení míst",
            "When you pick a seat, it is held for you for 10 minutes while you check out. Nobody else can buy it during that time, and if you leave, it frees up again automatically.",
            "Yer seçdiyiniz zaman o, ödəniş üçün 10 dəqiqə sizin üçün saxlanılır. Bu müddətdə heç kim onu ala bilməz, siz çıxsanız isə yer avtomatik boşalır.",
            "Když si vyberete místo, držíme ho pro vás 10 minut, než dokončíte nákup. Nikdo jiný ho mezitím nekoupí, a když odejdete, místo se samo uvolní."),
        new("find-events-faster", "Find events faster with new filters", "Yeni filtrlərlə tədbirləri daha tez tapın", "Nové filtry: najděte akci rychleji",
            "The events page now lets you search by name and filter by city, category and date, so you can find tonight's show in seconds.",
            "Tədbirlər səhifəsində indi adla axtarış, şəhər, kateqoriya və tarixə görə filtr var: bu axşamki şounu saniyələr içində tapın.",
            "Na stránce akcí teď můžete hledat podle názvu a filtrovat podle města, kategorie a data, takže dnešní představení najdete během pár vteřin."),
        new("caspian-hall-opening", "Caspian Concert Hall opens its doors", "Caspian Concert Hall qapılarını açır", "Caspian Concert Hall otevírá své dveře",
            "Our first venue in Baku opens with a mugham evening and a jazz weekend, with tickets in Azerbaijani, English and Czech.",
            "Bakıdakı ilk məkanımız muğam axşamı və caz həftəsonu ilə açılır; biletlər Azərbaycan, ingilis və çex dillərində satılır.",
            "Naše první místo v Baku otevírá večerem mugamu a jazzovým víkendem, vstupenky jsou k dispozici v ázerbájdžánštině, angličtině a češtině."),
        new("behind-the-scenes", "Behind the scenes at Vltava Arena", "Vltava Arena: pərdə arxasında", "Za oponou Vltava Arena",
            "How a crew of forty turns an empty arena into a concert stage in under twelve hours.",
            "Qırx nəfərlik komanda boş arenanı on iki saatdan az müddətdə konsert səhnəsinə necə çevirir.",
            "Jak čtyřicetičlenný tým promění prázdnou arénu v koncertní pódium za méně než dvanáct hodin."),
    ];

    // This one sells every seat, so the "sold out" state shows up in the demo
    private const string _soldOutSlug = "neon-harbour-live";

    public async Task<int> SeedAsync()
    {
        var categories = await _ensureCategoriesAsync();
        var halls = await _ensureVenuesAsync();
        var created = await _ensureEventsAsync(categories, halls);
        await _ensureSalesAsync();
        await _ensureNewsAsync();

        // repair the cached seat numbers of every event (also fixes rows written before RefreshSeatStatsAsync existed)
        foreach (var eventId in await _db.Events.IgnoreQueryFilters().Select(e => e.Id).ToListAsync())
            await _eventRepository.RefreshSeatStatsAsync(eventId);
        return created;
    }

    private async Task<Dictionary<string, Category>> _ensureCategoriesAsync()
    {
        var result = new Dictionary<string, Category>();
        foreach (var seed in _categories)
        {
            var category = await _db.Categories.Include(c => c.CategoryDetails).FirstOrDefaultAsync(c => c.Name == seed.En)
                           ?? _db.Categories.Add(new Category { Name = seed.En }).Entity;

            // Also repairs categories created without translations (e.g. the load-test "Concert")
            foreach (var (lang, name) in new[] { (LanguageType.English, seed.En), (LanguageType.Azerbaijani, seed.Az), (LanguageType.Czech, seed.Cs) })
            {
                if (category.CategoryDetails.All(d => d.LanguageId != (int)lang))
                    category.CategoryDetails.Add(new CategoryDetail { Name = name, LanguageId = (int)lang });
            }
            result[seed.Key] = category;
        }
        await _db.SaveChangesAsync();
        return result;
    }

    private async Task<Dictionary<string, Hall[]>> _ensureVenuesAsync()
    {
        var result = new Dictionary<string, Hall[]>();
        foreach (var seed in _venues)
        {
            var location = await _db.GeneralLocations.Include(l => l.Halls).FirstOrDefaultAsync(l => l.Name == seed.Name);
            if (location == null)
            {
                location = new GeneralLocation { Name = seed.Name, Address = seed.Address, City = seed.City, Country = seed.Country };
                foreach (var hallSeed in seed.Halls)
                    location.Halls.Add(_buildHall(hallSeed));
                _db.GeneralLocations.Add(location);
                _logger.LogInformation("Demo venue {Venue}: {Halls} hall(s)", seed.Name, seed.Halls.Length);
            }
            result[seed.Key] = seed.Halls.Select(h => location.Halls.First(x => x.Name == h.Name)).ToArray();
        }
        await _db.SaveChangesAsync();
        return result;
    }

    // VIP front rows, standard middle, balcony at the back (halls with 8+ rows)
    private static Hall _buildHall(HallSeed seed)
    {
        var hall = new Hall { Name = seed.Name };
        var vip = new SeatType { Name = "VIP", DefaultPrice = seed.BasePrice * 2.5m, Description = "Front rows", Hall = hall };
        var standard = new SeatType { Name = "Standard", DefaultPrice = seed.BasePrice, Hall = hall };
        var balcony = new SeatType { Name = "Balcony", DefaultPrice = Math.Round(seed.BasePrice * 0.7m, 2), Description = "Back rows", Hall = hall };

        for (var row = 1; row <= seed.Rows; row++)
        {
            var type = row <= 2 ? vip : seed.Rows >= 8 && row > seed.Rows - 2 ? balcony : standard;
            for (var col = 1; col <= seed.Columns; col++)
                hall.Seats.Add(new Seat { SeatName = $"R{row}-{col}", SeatRow = row, SeatColumn = col, SeatType = type, Hall = hall });
        }
        return hall;
    }

    private async Task<int> _ensureEventsAsync(Dictionary<string, Category> categories, Dictionary<string, Hall[]> halls)
    {
        var existing = await _db.Events.IgnoreQueryFilters().Select(e => e.Title).ToListAsync();
        var created = new List<(Event Event, EventSeed Seed)>();

        foreach (var seed in _events.Where(e => !existing.Contains(e.Title)))
        {
            var hall = halls[seed.Venue][seed.Hall];
            var prices = await _db.Seats.Where(s => s.HallId == hall.Id).Select(s => s.SeatType.DefaultPrice).Distinct().ToListAsync();

            var ev = new Event
            {
                Title = seed.Title,
                ImagePath = $"{_imageRoot}/events/{seed.Slug}.svg",
                LanguageId = (int)LanguageType.English,
                EventDate = DateTime.Today.AddDays(seed.DaysFromToday).AddHours(seed.Hour),
                PriceRange = string.Create(CultureInfo.InvariantCulture, $"{prices.Min():0.00}-{prices.Max():0.00}"),
                CategoryId = categories[seed.Category].Id,
                GeneralLocationId = hall.LocationId,
                HallId = hall.Id
            };
            _db.Events.Add(ev);
            created.Add((ev, seed));
        }
        await _db.SaveChangesAsync();

        foreach (var (ev, seed) in created)
        {
            var seatCount = await _eventSeatService.EnsureSeatsForEventAsync(ev.Id);
            var category = _categories.First(c => c.Key == seed.Category);
            var venue = _venues.First(v => v.Key == seed.Venue);
            var minPrice = decimal.Parse(ev.PriceRange.Split('-')[0], CultureInfo.InvariantCulture);

            foreach (var (lang, template) in new[] { (LanguageType.English, category.DescEn), (LanguageType.Azerbaijani, category.DescAz), (LanguageType.Czech, category.DescCs) })
            {
                _db.EventDetails.Add(new EventDetail
                {
                    EventId = ev.Id,
                    LanguageId = (int)lang,
                    Title = seed.Title,
                    Description = string.Format(template, seed.Title, venue.Name, venue.City),
                    TotalSeats = seatCount,
                    AvailableSeats = seatCount,
                    Price = minPrice
                });
            }
        }
        await _db.SaveChangesAsync();
        _logger.LogInformation("Demo events created: {Count} (of {Total})", created.Count, _events.Length);
        return created.Count;
    }

    // Confirmed reservations from the load-test user pool, written straight to the DB:
    // no SeatSold messages, so no emails go out to the fake addresses.
    private async Task _ensureSalesAsync()
    {
        var buyers = await _db.Users.Where(u => u.Email != null && u.Email.EndsWith("@bookit.test")).Select(u => u.Id).ToListAsync();
        if (buyers.Count == 0)
        {
            _logger.LogInformation("No @bookit.test users, skipping demo sales (run seed-loadtest first)");
            return;
        }

        var now = DateTime.UtcNow;
        var soldTotal = 0;
        foreach (var seed in _events)
        {
            var ev = await _db.Events.Include(e => e.EventDetail).FirstOrDefaultAsync(e => e.Title == seed.Title);
            // only skip events that already got demo sales; real purchases by other users don't count
            if (ev == null || await _db.Reservations.AnyAsync(r => r.EventId == ev.Id && buyers.Contains(r.UserId)))
                continue;

            // deterministic per event, so a fresh database gets the same numbers
            var rnd = new Random(seed.Slug.Sum(c => c));
            var seats = await _db.EventSeats.Include(x => x.Seat).ThenInclude(s => s.SeatType)
                .Where(x => x.EventId == ev.Id && x.Status == EventSeatStatus.Available)
                .ToListAsync();
            var ratio = seed.Slug == _soldOutSlug ? 1.0
                : seed.DaysFromToday < 0 ? 0.8 + rnd.NextDouble() * 0.2
                : 0.1 + rnd.NextDouble() * 0.65;
            var toSell = seats.OrderBy(_ => rnd.Next()).Take((int)Math.Round(seats.Count * ratio)).ToList();

            for (var i = 0; i < toSell.Count;)
            {
                var group = toSell.GetRange(i, Math.Min(rnd.Next(1, 5), toSell.Count - i));
                i += group.Count;

                var boughtAt = seed.DaysFromToday < 0
                    ? ev.EventDate.AddDays(-rnd.Next(1, 30))
                    : now.AddDays(-rnd.Next(0, 21)).AddHours(-rnd.Next(0, 24));
                var reservation = new Reservation
                {
                    EventId = ev.Id,
                    UserId = buyers[rnd.Next(buyers.Count)],
                    ReservationDate = boughtAt,
                    Status = ReservationStatus.Confirmed,
                    NumberOfTickets = group.Count,
                    TotalAmount = group.Sum(x => x.Seat.SeatType.DefaultPrice),
                    ReservationSeats = group.Select(x => new ReservationSeat { SeatId = x.SeatId, Price = x.Seat.SeatType.DefaultPrice }).ToList()
                };
                _db.Reservations.Add(reservation);

                foreach (var es in group)
                {
                    es.Status = EventSeatStatus.Sold;
                    es.Reservation = reservation;
                }
            }

            var soldOut = seats.Count > 0 && toSell.Count == seats.Count;
            ev.IsSoldOut = soldOut;
            foreach (var detail in ev.EventDetail)
            {
                detail.AvailableSeats = detail.TotalSeats - toSell.Count;
                detail.IsSoldOut = soldOut;
            }

            await _db.SaveChangesAsync();
            soldTotal += toSell.Count;
        }
        _logger.LogInformation("Demo seats sold: {Count}", soldTotal);
    }

    private async Task _ensureNewsAsync()
    {
        var existing = await _db.News.IgnoreQueryFilters().Select(n => n.ImagePath).ToListAsync();
        var added = 0;
        foreach (var seed in _news)
        {
            var image = $"{_imageRoot}/news/{seed.Slug}.svg";
            if (existing.Contains(image))
                continue;

            _db.News.Add(new News
            {
                ImagePath = image,
                NewsDetails =
                [
                    new NewsDetail { LanguageId = (int)LanguageType.English, Title = seed.En, Description = seed.BodyEn },
                    new NewsDetail { LanguageId = (int)LanguageType.Azerbaijani, Title = seed.Az, Description = seed.BodyAz },
                    new NewsDetail { LanguageId = (int)LanguageType.Czech, Title = seed.Cs, Description = seed.BodyCs },
                ]
            });
            added++;
        }
        await _db.SaveChangesAsync();
        _logger.LogInformation("Demo news created: {Count}", added);
    }
}
