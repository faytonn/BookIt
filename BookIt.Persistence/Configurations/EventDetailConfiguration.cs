using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class EventDetailConfiguration : IEntityTypeConfiguration<EventDetail>
{
    public void Configure(EntityTypeBuilder<EventDetail> builder)
    {
        throw new NotImplementedException();
    }
}
