using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class EventDetailConfiguration : IEntityTypeConfiguration<EventDetail>
{
    public void Configure(EntityTypeBuilder<EventDetail> builder)
    {
        builder.ToTable("EventDetails");

        builder.HasKey(ed => ed.Id);

        builder.Property(ed => ed.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(ed => ed.ImagePath)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(ed => ed.Description)
               .IsRequired();

        builder.Property(ed => ed.PriceRange)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(ed => ed.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(ed => ed.Location)
               .WithMany()
               .HasForeignKey(ed => ed.LocationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ed => ed.Hall)
               .WithMany(h => h.EventDetails) // i dont know if the inside should be empty or not
               .HasForeignKey(ed => ed.HallId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ed => ed.Event)
            .WithMany(e => e.EventDetail)
            .HasForeignKey(ed => ed.EventId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
