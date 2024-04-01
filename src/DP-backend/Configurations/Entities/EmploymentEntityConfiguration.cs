using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Configurations.Entities;

public class EmploymentEntityConfiguration : IEntityTypeConfiguration<Employment>
{
    public void Configure(EntityTypeBuilder<Employment> builder)
    {
        builder.ToTable(nameof(Employment));
        builder.OwnsOne(x => x.Employer, ownsBuilder =>
        {
            ownsBuilder.HasOne(x => x.Employer);
            ownsBuilder.Property(x => x.CustomCompanyName);
        });
    }
}