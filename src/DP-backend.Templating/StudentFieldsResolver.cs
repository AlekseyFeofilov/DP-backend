using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Templating;
using DP_backend.Domain.Templating.Employment;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Templating;

public class StudentFieldsResolver : ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>
{
    private readonly ApplicationDbContext _dbContext;

    private static readonly string[] ResolvingFields =
    [
        InternshipDiaryTemplate.Keys.StudentFullname,
        InternshipDiaryTemplate.Keys.StudentName
    ];

    public StudentFieldsResolver(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<string> CanResolveFields() => ResolvingFields;

    public async Task ResolveFields(string[] fieldToResolve, InternshipDiaryTemplateResolutionContext context, CancellationToken ct)
    {
        var student = await _dbContext.Students.GetUndeleted()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == context.InternshipDiaryRequest.StudentId, ct);
        if (student == null) throw new NotFoundException(context.InternshipDiaryRequest.StudentId.ToString());

        context.SetField(InternshipDiaryTemplate.Keys.StudentFullname, student.Name);
        context.SetField(InternshipDiaryTemplate.Keys.StudentName, InternshipDiaryTemplate.Formatting.ShortName(student.Name));
    }
}