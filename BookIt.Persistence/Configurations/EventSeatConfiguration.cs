using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class EventSeatConfiguration : IEntityTypeConfiguration<EventSeat>
{
    public void Configure(EntityTypeBuilder<EventSeat> builder)
    {
        builder.ToTable("EventSeats");

        builder.HasKey(es => es.Id);

        builder.Property(es => es.Status)
               .IsRequired();

        builder.Property(es => es.HeldByUserId)
               .HasMaxLength(450);

        builder.Property(es => es.RowVersion)
               .IsRowVersion();

        builder.HasOne(es => es.Event)
               .WithMany(e => e.EventSeats)
               .HasForeignKey(es => es.EventId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(es => es.Seat)
               .WithMany(s => s.EventSeats)
               .HasForeignKey(es => es.SeatId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(es => es.HeldBy)
               .WithMany()
               .HasForeignKey(es => es.HeldByUserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(es => es.Reservation)
               .WithMany()
               .HasForeignKey(es => es.ReservationId)
               .OnDelete(DeleteBehavior.SetNull);

        // a seat exists once per event
        builder.HasIndex(es => new { es.EventId, es.SeatId })
               .IsUnique();

        // seat-map and availability queries
        builder.HasIndex(es => new { es.EventId, es.Status });

        // expiry sweep
        builder.HasIndex(es => es.HeldUntil)
               .HasFilter("[Status] = 1");
    }
}
