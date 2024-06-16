using DP_backend.Domain.FileStorage;
using DP_backend.Domain.Templating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Database.EntityConfigurations;

public class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.ToTable("DocumentTemplate").HasKey(x => x.Id);

        builder.HasOne<FileHandle>().WithMany().HasForeignKey(x => x.TemplateFileId);

        builder.Property(x => x.BaseTemplateContext).HasColumnType("jsonb");

        builder.HasIndex(x => x.TemplateType);
    }
}