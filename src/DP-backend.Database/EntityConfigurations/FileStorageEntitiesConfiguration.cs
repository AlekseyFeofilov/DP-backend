using DP_backend.Domain.FileStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Database.EntityConfigurations;

public class FileStorageEntitiesConfiguration :
    IEntityTypeConfiguration<FileHandle>,
    IEntityTypeConfiguration<BucketHandle>,
    IEntityTypeConfiguration<FileEntityLink>
{
    public void Configure(EntityTypeBuilder<FileHandle> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Bucket).WithMany();
        builder.Navigation(x => x.Bucket).AutoInclude();
        builder.HasMany(x => x.Links).WithOne(x => x.File);
    }

    public void Configure(EntityTypeBuilder<BucketHandle> builder)
    {
        builder.HasKey(x => x.Id);
    }

    public void Configure(EntityTypeBuilder<FileEntityLink> builder)
    {
        builder.HasKey(x => new { x.FileId, x.EntityType, x.EntityId });
        builder.HasIndex(x => new { x.EntityType, x.EntityId }).IsUnique(false);
    }
}