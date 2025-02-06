using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {

        builder.ToTable("Messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Text)
               .IsRequired();

        builder.Property(m => m.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.HasOne(m => m.Chat)
               .WithMany(c => c.Messages)
               .HasForeignKey(m => m.ChatId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.User)
               .WithMany(u => u.Messages) // i added messages Icollection in the applicationuser for this, dont know if it is correct
               .HasForeignKey(m => m.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
