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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.AddSeedData();


        modelBuilder.Entity<Chat>()
                .HasOne(x => x.User)
                .WithMany() // or .WithMany(u => u.Chats) if you have a navigation property
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(x => x.Moderator)
                .WithMany()
                .HasForeignKey(x => x.ModeratorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Chat)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Reservations)
                .WithOne(r => r.Event)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Reservation)
                .WithOne(r => r.PaymentTransaction)
                .HasForeignKey<Reservation>(r => r.PaymentTransactionId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Chat>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Event>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<EventDetail>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Hall>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<News>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Notification>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PaymentTransaction>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Reservation>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Review>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<WaitlistEntry>().HasQueryFilter(x => !x.IsDeleted);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_entityInterceptor);

            base.OnConfiguring(optionsBuilder);
        }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<CancellationRefund> CancellationRefunds { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<EventDetail> EventDetails { get; set; } = null!;
    public DbSet<EventSeatType> EventSeatTypes { get; set; } = null!;
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

