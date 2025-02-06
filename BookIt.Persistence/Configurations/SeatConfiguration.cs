using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SeatName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(s => s.SeatColumn)
               .IsRequired();

        builder.Property(s => s.SeatRow)
               .IsRequired();

        builder.Property(s => s.IsReserved)
               .IsRequired()
               .HasDefaultValue(false);

        builder.HasOne(s => s.SeatType)
               .WithMany() // if SeatType has a Seats collection change it but i dont know
               .HasForeignKey(s => s.SeatTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Hall)
               .WithMany(h => h.Seats) 
               .HasForeignKey(s => s.HallId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.SeatRow, s.SeatColumn, s.HallId })
               .IsUnique();
    }
}
