using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Principal;
using DP_backend.Domain.Employment;
using DP_backend.Migrations;

namespace DP_backend.Helpers
{
    public static class OperationUpdateHelper
    {
        public static async Task CatchOperationUpdate(ChangeTracker changeTracker, ApplicationDbContext context)
        {
            var modifiedEntries = changeTracker.Entries<EmploymentVariant>();
            var modifiedEntries2 = changeTracker.Entries<Employment>();

            foreach (var entity in modifiedEntries)
            {
                if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
                {
                    var student = await context.Students
                        .Include(x => x.Employment)
                        .Include(x => x.EmploymentVariants)
                        .FirstOrDefaultAsync(x => x.Id == entity.Entity.StudentId);
                    if (student.Employment != null && student.Employment.Status == EmploymentStatus.Verified)
                    {
                        student.Status = StudentStatus.Employed;
                    }
                    else if (student.Employment != null)
                    {
                        student.Status = StudentStatus.EmployedNotVerified;
                    }
                    else if (student.EmploymentVariants.Any(x => x.Status == EmploymentVariantStatus.Offered))
                    {
                        student.Status = StudentStatus.GetAnOffer;
                    }
                    else if (student.EmploymentVariants.Any(x => x.Status == EmploymentVariantStatus.Interviewed))
                    {
                        student.Status = StudentStatus.PassedTheInterview;
                    }
                    else if (student.EmploymentVariants.Any())
                    {
                        student.Status = StudentStatus.CompaniesChose;
                    }
                    else
                    {
                        student.Status = StudentStatus.None;
                    }
                }
                await context.SaveChangesAsync();
            }

            foreach (var entity in modifiedEntries2)
            {
                if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
                {
                    var student = await context.Students
                        .Include(x => x.Employment)
                        .Include(x => x.EmploymentVariants)
                        .FirstOrDefaultAsync(x => x.Id == entity.Entity.StudentId);
                    if (student.Employment != null && student.Employment.Status == EmploymentStatus.Verified)
                    {
                        student.Status = StudentStatus.Employed;
                    }
                    else if (student.Employment != null)
                    {
                        student.Status = StudentStatus.EmployedNotVerified;
                    }
                    else if (student.EmploymentVariants.Any(x => x.Status == EmploymentVariantStatus.Offered))
                    {
                        student.Status = StudentStatus.GetAnOffer;
                    }
                    else if (student.EmploymentVariants.Any(x => x.Status == EmploymentVariantStatus.Interviewed))
                    {
                        student.Status = StudentStatus.PassedTheInterview;
                    }
                    else if (student.EmploymentVariants.Any())
                    {
                        student.Status = StudentStatus.CompaniesChose;
                    }
                    else
                    {
                        student.Status = StudentStatus.None;
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
