using System.Reflection;
using DP_backend.Common;
using DP_backend.Database.EntityConfigurations;
using DP_backend.Database.Helpers;
using DP_backend.Domain.Employment;
using DP_backend.Domain.FileStorage;
using DP_backend.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Database
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
        public virtual DbSet<EmploymentRequest> EmploymentRequests { get; set; }
        public virtual DbSet<InternshipRequest> InternshipRequests { get; set; }

        public virtual DbSet<FileHandle> FileHandles { get; set; }
        public virtual DbSet<BucketHandle> BucketHandles { get; set; }
        public virtual DbSet<FileEntityLink> FileEntityLinks { get; set; }
        
        public virtual DbSet<InternshipDiaryRequest> InternshipDiaryRequests { get; set; }
        public virtual DbSet<CourseWorkRequest> CourseWorkRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyGlobalFilters<IBaseEntity>(e => e.DeleteDateTime == null);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(EmploymentEntityConfiguration))!);
        }

        public override int SaveChanges()
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return base.SaveChanges();
        }

        public int SaveChanges(DateTime dateTime)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess, DateTime dateTime)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            await OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker);
            await OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(DateTime dateTime, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            BaseEntityTimestampHelper.SetTimestamps(ChangeTracker, dateTime);
            await OperationUpdateHelper.CatchOperationUpdate(ChangeTracker, this);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}