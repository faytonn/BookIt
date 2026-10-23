using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class ReservationSeatConfiguration : IEntityTypeConfiguration<ReservationSeat>
{
    public void Configure(EntityTypeBuilder<ReservationSeat> builder)
    {
        builder.ToTable("ReservationSeats");

        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(rs => rs.Reservation)
               .WithMany(r => r.ReservationSeats)
               .HasForeignKey(rs => rs.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rs => rs.Seat)
               .WithMany(s => s.ReservationSeats)
               .HasForeignKey(rs => rs.SeatId)
               .OnDelete(DeleteBehavior.Restrict);

        // a seat can appear only once inside one reservation
        builder.HasIndex(rs => new { rs.ReservationId, rs.SeatId })
               .IsUnique();
    }
}
