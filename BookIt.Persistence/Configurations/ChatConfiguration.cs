using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BookIt.Persistence.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> modelBuilder)
    {
        modelBuilder
            .HasOne(x => x.User)
            .WithMany(/*x.Chats*/) 
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .HasOne(x => x.Moderator)
            .WithMany(/*x => x.ModeratedChats*/) 
            .HasForeignKey(x => x.ModeratorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
