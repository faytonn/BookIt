using BookIt.Domain.Entities;
using BookIt.Persistence.DataInitializers;
using BookIt.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace BookIt.Persistence.Contexts;


public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly BaseEntityInterceptor _entityInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options,
                        BaseEntityInterceptor entityInterceptor)
        : base(options)
    {
        _entityInterceptor = entityInterceptor;
    }

    //public AppDbContext()
    //{

    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.AddSeedData();


        modelBuilder.Entity<Event>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<EventDetail>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Hall>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Chat>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Message>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<News>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<GeneralLocation>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Review>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<WaitlistEntry>().HasQueryFilter(x => !x.IsDeleted);
        // soft delete like every other auditable entity (it used to filter on IsRefunded, which hid paid-out refunds)
        modelBuilder.Entity<CancellationRefund>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<SeatType>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(x => !x.IsDeleted);

        // Dependents of a filtered principal need a matching filter, otherwise EF warns and
        // a required navigation can come back null for rows whose parent is soft-deleted.
        modelBuilder.Entity<Seat>().HasQueryFilter(x => !x.IsDeleted && !x.Hall.IsDeleted);
        modelBuilder.Entity<Reservation>().HasQueryFilter(x => !x.IsDeleted && !x.Event!.IsDeleted);
        modelBuilder.Entity<ReservationSeat>().HasQueryFilter(x => !x.IsDeleted && !x.Reservation!.IsDeleted && !x.Reservation.Event!.IsDeleted);
        modelBuilder.Entity<EventSeat>().HasQueryFilter(x => !x.Event.IsDeleted && !x.Seat.IsDeleted && !x.Seat.Hall.IsDeleted);
        modelBuilder.Entity<CategoryDetail>().HasQueryFilter(x => !x.Category.IsDeleted);
        modelBuilder.Entity<NewsDetail>().HasQueryFilter(x => !x.News!.IsDeleted);
        modelBuilder.Entity<NotificationDetail>().HasQueryFilter(x => !x.Notification!.IsDeleted);


    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_entityInterceptor);

        //optionsBuilder.UseSqlServer("DefaultConnection");

        base.OnConfiguring(optionsBuilder);
    }


    //public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<CancellationRefund> CancellationRefunds { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CategoryDetail> CategoryDetails { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<EventDetail> EventDetails { get; set; } = null!;
    public DbSet<EventSeat> EventSeats { get; set; } = null!;
    public DbSet<GeneralLocation> GeneralLocations { get; set; } = null!;
    public DbSet<Hall> Halls { get; set; } = null!;
    public DbSet<Language> Languages { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<NewsDetail> NewsDetails { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<NotificationDetail> NotificationDetails { get; set; } = null!;
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Seat> Seats { get; set; } = null!;
    public DbSet<SeatType> SeatTypes { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<SettingDetail> SettingDetails { get; set; } = null!;
    public DbSet<Slider> Sliders { get; set; } = null!;
    public DbSet<WaitlistEntry> WaitlistEntries { get; set; } = null!;
}

