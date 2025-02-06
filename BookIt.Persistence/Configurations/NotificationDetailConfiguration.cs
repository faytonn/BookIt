using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class NotificationDetailConfiguration : IEntityTypeConfiguration<NotificationDetail>
{
    public void Configure(EntityTypeBuilder<NotificationDetail> builder)
    {
        throw new NotImplementedException();
    }
}
