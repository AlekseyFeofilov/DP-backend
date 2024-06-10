using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Database.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(nameof(Comment));
        builder.HasIndex(x => new { x.TargetEntityType, x.TargetEntityId });

        // ограничение FK выглядит излищним 
        // builder.HasOne<EntityType>().WithMany().HasForeignKey(x => x.TargetEntityType);
    }
}