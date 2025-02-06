using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Settings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Key)
               .HasMaxLength(100);

        builder.HasIndex(s => s.Key)
               .IsUnique();

        builder.HasMany(s => s.SettingDetails)
               .WithOne(sd => sd.Setting)
               .HasForeignKey(sd => sd.SettingId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
