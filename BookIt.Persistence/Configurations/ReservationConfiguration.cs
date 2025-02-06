using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.Property(r => r.NumberOfTickets)
               .IsRequired();

        builder.Property(r => r.ReservationDate)
               .IsRequired();

        builder.Property(r => r.TotalAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(r => r.Event)
               .WithMany(e => e.Reservations)
               .HasForeignKey(r => r.EventId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
               .WithMany() 
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.PaymentTransaction)
               .WithOne(pt => pt.Reservation)
               .HasForeignKey<Reservation>(r => r.PaymentTransactionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
