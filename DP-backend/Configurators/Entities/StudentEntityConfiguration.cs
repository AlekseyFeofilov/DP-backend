using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DP_backend.Configurators.Entities;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasOne<User>().WithOne().HasForeignKey<Student>(x => x.Id);
        builder.HasMany(x => x.EmploymentVariants).WithOne(x => x.Student);
        builder.HasOne(x => x.Employment);
    }
}