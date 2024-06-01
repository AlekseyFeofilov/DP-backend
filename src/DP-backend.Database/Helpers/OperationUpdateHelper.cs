using DP_backend.Domain.Employment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DP_backend.Database.Helpers
{
    public static class OperationUpdateHelper
    {
        public static async Task CatchOperationUpdate(ChangeTracker changeTracker, ApplicationDbContext context)
        {
            var modifiedEntries = changeTracker.Entries<EmploymentVariant>();
            var modifiedEntries2 = changeTracker.Entries<Employment>();
            var modifiedEntries3 = changeTracker.Entries<EmploymentRequest>();
            var modifiedEntries4 = changeTracker.Entries<InternshipRequest>();
            foreach (var entity in modifiedEntries)
            {
                if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
                {
                    var student = await context.Students
                        .Include(x => x.Employments)
                        .Include(x => x.EmploymentVariants)
                        .Include(x => x.InternshipRequests)
                        .Include(x=>x.EmploymentRequests)
                        .Include(x=>x.InternshipRequests)
                        .FirstOrDefaultAsync(x => x.Id == entity.Entity.StudentId);
                    if (student.Employments.Any(x=>x.Status== EmploymentStatus.Active))
                    {
                        student.Status = StudentStatus.Employed;
                    }
                    else if (student.EmploymentRequests.Any(x => x.Status == EmploymentRequestStatus.NonVerified))
                    {
                        student.Status = StudentStatus.EmployedNotVerified;
                    }
                    else if (student.EmploymentVariants
                        .Any(x => 
                        (x.Status == EmploymentVariantStatus.OfferAccepted ||
                        x.Status == EmploymentVariantStatus.OfferRefused || 
                        x.Status == EmploymentVariantStatus.OfferPending)
                        && x.InternshipRequest.Status==InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.GetAnOffer;
                    }
                    else if (student.EmploymentVariants.Any(x =>( x.Status == EmploymentVariantStatus.Interviewed) && x.InternshipRequest.Status==InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.PassedTheInterview;
                    }
                    else if (student.InternshipRequests.Any(x=>x.Status==InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.CompaniesChose;
                    }
                    else
                    {
                        student.Status = StudentStatus.None;
                    }
                    changeTracker.TrackGraph(student, e =>
                    e.Entry.State = EntityState.Modified
                    );
                }
            }

            foreach (var entity in modifiedEntries2)
            {

                if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
                {
                    var student = await context.Students
                        .Include(x => x.Employments)
                        .Include(x => x.EmploymentVariants)
                        .Include(x => x.InternshipRequests)
                        .Include(x => x.EmploymentRequests)
                        .Include(x => x.InternshipRequests)
                        .FirstOrDefaultAsync(x => x.Id == entity.Entity.StudentId);
                    if (student.Employments.Any(x => x.Status == EmploymentStatus.Active))
                    {
                        student.Status = StudentStatus.Employed;
                    }
                    else if (student.EmploymentRequests.Any(x => x.Status == EmploymentRequestStatus.NonVerified))
                    {
                        student.Status = StudentStatus.EmployedNotVerified;
                    }
                    else if (student.EmploymentVariants
                        .Any(x =>
                        (x.Status == EmploymentVariantStatus.OfferAccepted ||
                        x.Status == EmploymentVariantStatus.OfferRefused ||
                        x.Status == EmploymentVariantStatus.OfferPending)
                        && x.InternshipRequest.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.GetAnOffer;
                    }
                    else if (student.EmploymentVariants.Any(x => (x.Status == EmploymentVariantStatus.Interviewed) && x.InternshipRequest.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.PassedTheInterview;
                    }
                    else if (student.InternshipRequests.Any(x => x.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.CompaniesChose;
                    }
                    else
                    {
                        student.Status = StudentStatus.None;
                    }
                    if (entity.Entity.Status == EmploymentStatus.Active)
                    {
                        entity.Entity.EndDate = null;
                    }
                    else if(entity.Entity.Status == EmploymentStatus.InActive && entity.Entity.EndDate == null)
                    {
                        entity.Entity.EndDate = DateTime.UtcNow;
                    }
                    changeTracker.TrackGraph(student, e =>
                    e.Entry.State = EntityState.Modified
                    );
                }
            }

            foreach (var entity in modifiedEntries3)
            {
                if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
                {
                    var student = await context.Students
                        .Include(x => x.Employments)
                        .Include(x => x.EmploymentVariants)
                        .Include(x => x.InternshipRequests)
                        .Include(x => x.EmploymentRequests)
                        .Include(x => x.InternshipRequests)
                        .FirstOrDefaultAsync(x => x.Id == entity.Entity.StudentId);
                    if (student.Employments.Any(x => x.Status == EmploymentStatus.Active))
                    {
                        student.Status = StudentStatus.Employed;
                    }
                    else if (student.EmploymentRequests.Any(x => x.Status == EmploymentRequestStatus.NonVerified))
                    {
                        student.Status = StudentStatus.EmployedNotVerified;
                    }
                    else if (student.EmploymentVariants
                        .Any(x =>
                        (x.Status == EmploymentVariantStatus.OfferAccepted ||
                        x.Status == EmploymentVariantStatus.OfferRefused ||
                        x.Status == EmploymentVariantStatus.OfferPending)
                        && x.InternshipRequest.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.GetAnOffer;
                    }
                    else if (student.EmploymentVariants.Any(x => (x.Status == EmploymentVariantStatus.Interviewed) && x.InternshipRequest.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.PassedTheInterview;
                    }
                    else if (student.InternshipRequests.Any(x => x.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.CompaniesChose;
                    }
                    else
                    {
                        student.Status = StudentStatus.None;
                    }
                    changeTracker.TrackGraph(student, e =>
                    e.Entry.State = EntityState.Modified
                    );
                }
            }

            foreach (var entity in modifiedEntries4)
            {
                if (entity.State == EntityState.Modified || entity.State == EntityState.Added)
                {
                    var student = await context.Students
                        .Include(x => x.Employments)
                        .Include(x => x.EmploymentVariants)
                        .Include(x => x.InternshipRequests)
                        .Include(x => x.EmploymentRequests)
                        .Include(x => x.InternshipRequests)
                        .FirstOrDefaultAsync(x => x.Id == entity.Entity.StudentId);
                    if (student.Employments.Any(x => x.Status == EmploymentStatus.Active))
                    {
                        student.Status = StudentStatus.Employed;
                    }
                    else if (student.EmploymentRequests.Any(x => x.Status == EmploymentRequestStatus.NonVerified))
                    {
                        student.Status = StudentStatus.EmployedNotVerified;
                    }
                    else if (student.EmploymentVariants
                        .Any(x =>
                        (x.Status == EmploymentVariantStatus.OfferAccepted ||
                        x.Status == EmploymentVariantStatus.OfferRefused ||
                        x.Status == EmploymentVariantStatus.OfferPending)
                        && x.InternshipRequest.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.GetAnOffer;
                    }
                    else if (student.EmploymentVariants.Any(x => (x.Status == EmploymentVariantStatus.Interviewed) && x.InternshipRequest.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.PassedTheInterview;
                    }
                    else if (student.InternshipRequests.Any(x => x.Status == InternshipStatus.Accepted))
                    {
                        student.Status = StudentStatus.CompaniesChose;
                    }
                    else
                    {
                        student.Status = StudentStatus.None;
                    }
                    changeTracker.TrackGraph(student, e =>
                    e.Entry.State = EntityState.Modified
                    );
                }
            }
        }
    }
}
