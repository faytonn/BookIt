using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BookIt.Persistence.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
               .IsRequired()
               .HasMaxLength(450); 

        builder.Property(c => c.ModeratorId)
               .HasMaxLength(450);

        // Relationships
        builder.HasOne(c => c.User)
               .WithMany(u => u.Chats) 
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Moderator)
               .WithMany() 
               .HasForeignKey(c => c.ModeratorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
