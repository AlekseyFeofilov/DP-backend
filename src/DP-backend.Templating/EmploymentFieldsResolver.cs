using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Domain.Templating;
using DP_backend.Domain.Templating.Employment;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Templating;

public class EmploymentFieldsResolver : ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>
{
    private readonly ApplicationDbContext _dbContext;

    private static readonly string[] ResolvingFields =
    [
        InternshipDiaryTemplate.Keys.EmploymentName,
        InternshipDiaryTemplate.Keys.ManagerFromEmployment,
        InternshipDiaryTemplate.Keys.EmploymentDelegate,
        InternshipDiaryTemplate.Keys.StudentIndividualTask
    ];

    public EmploymentFieldsResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<string> CanResolveFields() => ResolvingFields;

    public async Task ResolveFields(string[] fieldToResolve, InternshipDiaryTemplateResolutionContext context, CancellationToken ct)
    {
        var employer = await _dbContext.Employments.GetUndeleted()
            .AsNoTracking()
            .Where(x => x.StudentId == context.InternshipDiaryRequest.StudentId && x.Status == EmploymentStatus.Active)
            .Select(x => x.Employer)
            .FirstOrDefaultAsync(ct);
        if (employer == null) throw new NotFoundException($"Трудоустройство для заявки для на дневник практики ({context.InternshipDiaryRequest.Id}) не найден");

        context.SetField(InternshipDiaryTemplate.Keys.EmploymentName, employer.CompanyName);
        context.SetField(InternshipDiaryTemplate.Keys.EmploymentDelegate, employer.AuthorizedDelegate);

        context.SetField(InternshipDiaryTemplate.Keys.ManagerFromEmployment,
            string.IsNullOrWhiteSpace(context.InternshipDiaryRequest.ManagerFromEmployment) ? employer.AuthorizedDelegate : context.InternshipDiaryRequest.ManagerFromEmployment);

        context.SetField(InternshipDiaryTemplate.Keys.StudentIndividualTask,
            string.IsNullOrWhiteSpace(context.InternshipDiaryRequest.StudentIndividualTask) ? employer.CompanyName : context.InternshipDiaryRequest.StudentIndividualTask);
    }
}