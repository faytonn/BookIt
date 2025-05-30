﻿using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

internal class CancellationRefundConfiguration : IEntityTypeConfiguration<CancellationRefund>
{
    public void Configure(EntityTypeBuilder<CancellationRefund> builder)
    {
        builder.ToTable(nameof(CancellationRefund));

        builder.HasKey(c => c.Id);

        builder.Property(c => c.RefundAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(cr => cr.CancellationDate)
               .IsRequired();

        builder.Property(cr => cr.IsRefunded)
               .IsRequired();

        builder.HasOne(cr => cr.Reservation)
               .WithOne(r => r.CancellationRefund) // or empty inside the bracket idk yet
               .HasForeignKey<CancellationRefund>(cr => cr.ReservationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
