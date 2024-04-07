using System.Reflection;
using DP_backend.Configurations.Entities;
using DP_backend.Domain.Employment;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DP_backend.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using DP_backend.Helpers;

namespace DP_backend
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<EmploymentVariant> EmploymentVariants { get; set; }
        public virtual DbSet<Employment> Employments { get; set; }
        // todo : see InternshipReport
        // public virtual DbSet<InternshipReport> InternshipReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(EmploymentEntityConfiguration))!);
        }

        public override int SaveChanges()
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            return base.SaveChanges();
        }

        public int SaveChanges(DateTime dateTime)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess, DateTime dateTime)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public Task<int> SaveChangesAsync(DateTime dateTime, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}