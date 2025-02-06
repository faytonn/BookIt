using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.HasMany(n => n.NotificationDetails)
               .WithOne(nd => nd.Notification)
               .HasForeignKey(nd => nd.NotificationId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
