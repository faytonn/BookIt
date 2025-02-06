using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class NotificationDetailConfiguration : IEntityTypeConfiguration<NotificationDetail>
{
    public void Configure(EntityTypeBuilder<NotificationDetail> builder)
    {
        builder.ToTable("NotificationDetails");

        builder.HasKey(nd => nd.Id);

        builder.Property(nd => nd.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(nd => nd.Description)
               .IsRequired();

        builder.HasOne(nd => nd.Notification)
               .WithMany(n => n.NotificationDetails)
               .HasForeignKey(nd => nd.NotificationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(nd => nd.Language)
               .WithMany() 
               .HasForeignKey(nd => nd.LanguageId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
