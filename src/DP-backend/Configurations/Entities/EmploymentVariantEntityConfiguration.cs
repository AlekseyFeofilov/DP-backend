using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Configurations.Entities;

public class EmploymentVariantEntityConfiguration : IEntityTypeConfiguration<EmploymentVariant>
{
    public void Configure(EntityTypeBuilder<EmploymentVariant> builder)
    {
        builder.ToTable(nameof(EmploymentVariant));
        builder.OwnsOne(x => x.Employer, ownsBuilder =>
        {
            ownsBuilder.HasOne(x => x.Employer);
            ownsBuilder.Property(x => x.CustomCompanyName);
        });
    }
}