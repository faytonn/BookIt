using BookIt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIt.Persistence.Configurations;

public class SettingDetailConfiguration : IEntityTypeConfiguration<SettingDetail>
{
    public void Configure(EntityTypeBuilder<SettingDetail> builder)
    {
        throw new NotImplementedException();
    }
}
