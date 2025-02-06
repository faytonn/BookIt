using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.LangaugeName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(l => l.IsoCode)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(l => l.ImagePath)
               .IsRequired()
               .HasMaxLength(250);

        builder.HasIndex(l => l.IsoCode)
               .IsUnique();
    }
}
