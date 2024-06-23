using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Database.EntityConfigurations;

public class InternshipDiaryRequestCommentEntityConfiguration : IEntityTypeConfiguration<InternshipDiaryRequest>
{
    public void Configure(EntityTypeBuilder<InternshipDiaryRequest> builder)
    {
        builder.HasKey(x => x.Id);
    }
}