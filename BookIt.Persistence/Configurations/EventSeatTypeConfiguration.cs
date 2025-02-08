using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class EventSeatTypeConfiguration : IEntityTypeConfiguration<EventDetailSeatType>
{
    public void Configure(EntityTypeBuilder<EventDetailSeatType> builder)
    {
        builder.ToTable("EventSeatTypes");

        builder.HasKey(est => est.Id);

        builder.Property(est => est.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(est => est.AdditionalDetails)
               .HasMaxLength(500);

        builder.HasOne(est => est.EventDetail)
               .WithMany() // i can maybe make an icollection for event seat types
               .HasForeignKey(est => est.EventDetailId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(est => est.SeatType)
               .WithMany(st => st.EventSeatTypes)
               .HasForeignKey(est => est.SeatTypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
