using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
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
            .Where(x => x.InternshipRequestId == context.InternshipDiaryRequest.InternshipRequestId)
            .Select(x => x.Employer)
            .FirstOrDefaultAsync(ct);
        if (employer == null) throw new NotFoundException($"Employment by InternshipDiaryRequest ({context.InternshipDiaryRequest.Id})");

        context.SetField(InternshipDiaryTemplate.Keys.EmploymentName, employer.CompanyName);

        // todo : корректно ли использовать здесь Tutor
        context.SetField(InternshipDiaryTemplate.Keys.ManagerFromEmployment, employer.Tutor);
        context.SetField(InternshipDiaryTemplate.Keys.EmploymentDelegate, employer.Tutor);
    }
}