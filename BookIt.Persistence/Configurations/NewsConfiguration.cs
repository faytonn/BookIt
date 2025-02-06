using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable("News");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.ImagePath)
               .HasMaxLength(250);

        builder.HasMany(n => n.NewsDetails)
               .WithOne(nd => nd.News)
               .HasForeignKey(nd => nd.NewsId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
