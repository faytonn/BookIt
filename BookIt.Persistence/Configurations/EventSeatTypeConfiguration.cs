using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class EventSeatTypeConfiguration : IEntityTypeConfiguration<EventSeatType>
{
    public void Configure(EntityTypeBuilder<EventSeatType> builder)
    {
        throw new NotImplementedException();
    }
}
