using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class NewsDetailConfiguration : IEntityTypeConfiguration<NewsDetail>
{
    public void Configure(EntityTypeBuilder<NewsDetail> builder)
    {
        builder.ToTable("NewsDetails");

        builder.HasKey(nd => nd.Id);

        builder.Property(nd => nd.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(nd => nd.Description)
               .IsRequired();

        builder.HasOne(nd => nd.News)
               .WithMany(n => n.NewsDetails)
               .HasForeignKey(nd => nd.NewsId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(nd => nd.Language)
               .WithMany() 
               .HasForeignKey(nd => nd.LanguageId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
