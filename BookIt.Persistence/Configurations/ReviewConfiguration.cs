using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(t => {t.HasCheckConstraint("CK_Review_Rating_Range", "[Rating] >= 0 AND [Rating] <= 5");});

        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId)
               .IsRequired()
               .HasMaxLength(450);

        builder.Property(r => r.Rating)
               .IsRequired();

        builder.Property(r => r.Stars)
               .IsRequired();

        builder.Property(r => r.ReviewDate)
               .IsRequired();

        builder.Property(r => r.Comment)
               .HasMaxLength(1000);

        builder.HasOne(r => r.User)
               .WithMany()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);

    }
}
