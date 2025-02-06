using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

internal class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder
           .HasOne(pt => pt.Reservation)
           .WithOne(r => r.PaymentTransaction)
           .HasForeignKey<Reservation>(r => r.PaymentTransactionId)
           .OnDelete(DeleteBehavior.Cascade);


    }
}
