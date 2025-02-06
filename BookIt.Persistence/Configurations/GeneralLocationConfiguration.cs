using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class GeneralLocationConfiguration : IEntityTypeConfiguration<GeneralLocation>
{
    public void Configure(EntityTypeBuilder<GeneralLocation> builder)
    {
        throw new NotImplementedException();
    }
}
