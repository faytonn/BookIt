using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class NewsDetailConfiguration : IEntityTypeConfiguration<NewsDetail>
{
    public void Configure(EntityTypeBuilder<NewsDetail> builder)
    {
        throw new NotImplementedException();
    }
}
