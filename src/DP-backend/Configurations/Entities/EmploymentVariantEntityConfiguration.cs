using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Configurations.Entities;

public class EmploymentVariantEntityConfiguration : IEntityTypeConfiguration<EmploymentVariant>
{
    public void Configure(EntityTypeBuilder<EmploymentVariant> builder)
    {
        builder.ToTable(nameof(EmploymentVariant));
        builder.HasOne(x => x.InternshipRequest);

        builder.Navigation(x => x.Student).AutoInclude();
    }
}