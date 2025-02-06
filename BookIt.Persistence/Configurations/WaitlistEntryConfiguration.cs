using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class WaitlistEntryConfiguration : IEntityTypeConfiguration<WaitlistEntry>
{
    public void Configure(EntityTypeBuilder<WaitlistEntry> builder)
    {
        builder.ToTable("WaitlistEntries");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.Property(w => w.RequestedDate)
               .IsRequired();

        builder.Property(w => w.IsNotified)
               .IsRequired()
               .HasDefaultValue(false);

        builder.HasOne(w => w.Event)
               .WithMany() // Adjust if Event has a WaitlistEntries collection
               .HasForeignKey(w => w.EventId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(w => w.User)
               .WithMany() // Adjust if ApplicationUser has a WaitlistEntries collection
               .HasForeignKey(w => w.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
