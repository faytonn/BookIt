using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class SeatTypeConfiguration : IEntityTypeConfiguration<SeatType>
{
    public void Configure(EntityTypeBuilder<SeatType> builder)
    {
        builder.ToTable("SeatTypes");

        builder.HasKey(st => st.Id);

        builder.Property(st => st.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(st => st.DefaultPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(st => st.Description)
               .HasMaxLength(500);

        builder.HasOne(st => st.Hall)
               .WithMany() 
               .HasForeignKey(st => st.HallId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
