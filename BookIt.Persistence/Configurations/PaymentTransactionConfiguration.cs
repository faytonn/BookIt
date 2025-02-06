using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

internal class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(pt => pt.PaymentDate)
               .IsRequired();

        builder.Property(pt => pt.PaymentMethod)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(pt => pt.TransactionReference)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasOne(pt => pt.Reservation)
               .WithOne(r => r.PaymentTransaction)
               .HasForeignKey<Reservation>(r => r.PaymentTransactionId)
               .OnDelete(DeleteBehavior.Cascade);


    }
}
