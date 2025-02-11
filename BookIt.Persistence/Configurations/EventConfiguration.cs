using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.ImagePath)
               .IsRequired()
               .HasMaxLength(450);

        builder.Property(e => e.PriceRange)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.EventDate)
               .IsRequired();

        builder.HasOne(e => e.GeneralLocation)
               .WithMany()
               .HasForeignKey(e => e.GeneralLocationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.EventDetail)
        .WithOne(ed => ed.Event)
        .HasForeignKey(ed => ed.EventId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Reservations)
               .WithOne(r => r.Event)
               .HasForeignKey(r => r.EventId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
