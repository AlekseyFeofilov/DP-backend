using DP_backend.Common;
using DP_backend.Common.EntityType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Database.EntityConfigurations;

public class EntityTypeConfiguration : IEntityTypeConfiguration<EntityType>
{
    public void Configure(EntityTypeBuilder<EntityType> builder)
    {
        builder.ToTable("EntityType").HasKey(x => x.Id);
    }
}