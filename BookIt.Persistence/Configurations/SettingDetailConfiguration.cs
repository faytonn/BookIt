using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class SettingDetailConfiguration : IEntityTypeConfiguration<SettingDetail>
{
    public void Configure(EntityTypeBuilder<SettingDetail> builder)
    {
        builder.ToTable("SettingDetails");

        builder.HasKey(sd => sd.Id);

        builder.Property(sd => sd.Value)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasOne(sd => sd.Setting)
               .WithMany(s => s.SettingDetails)
               .HasForeignKey(sd => sd.SettingId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sd => sd.Language)
               .WithMany() // Adjust if Language has a SettingDetails collection
               .HasForeignKey(sd => sd.LanguageId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sd => new { sd.SettingId, sd.LanguageId });
    }
}

