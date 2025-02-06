using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("Halls");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(h => h.IsDeleted)
               .HasDefaultValue(false);

        builder.HasOne(h => h.Location)
               .WithMany(gl => gl.Halls) //can be empty too, not sure
               .HasForeignKey(h => h.LocationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
