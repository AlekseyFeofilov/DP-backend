using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Configurators.Entities;

public class EmploymentEntityConfiguration : IEntityTypeConfiguration<Employment>
{
    public void Configure(EntityTypeBuilder<Employment> builder)
    {
        builder.OwnsOne(x => x.Employer, ownsBuilder =>
        {
            ownsBuilder.HasOne(x => x.Employer);
            ownsBuilder.Property(x => x.CustomCompanyName);
        });
    }
}