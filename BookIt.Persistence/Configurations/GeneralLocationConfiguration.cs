using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class GeneralLocationConfiguration : IEntityTypeConfiguration<GeneralLocation>
{
    public void Configure(EntityTypeBuilder<GeneralLocation> builder)
    {
        builder.ToTable("GeneralLocations");

        builder.HasKey(gl => gl.Id);

        builder.Property(gl => gl.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(gl => gl.Address)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(gl => gl.City)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(gl => gl.Country)
               .IsRequired()
               .HasMaxLength(100);

    }
}
