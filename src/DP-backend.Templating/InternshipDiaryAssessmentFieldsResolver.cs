using DP_backend.Domain.Templating;
using DP_backend.Domain.Templating.Employment;

namespace DP_backend.Templating;

public class InternshipDiaryAssessmentFieldsResolver : ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>
{
    private static readonly string[] ResolvingFields =
    [
        InternshipDiaryTemplate.Keys.AssessmentFromEmploymentText,
        InternshipDiaryTemplate.Keys.AssessmentMarkFromEmployment,
        InternshipDiaryTemplate.Keys.AssessmentFromEmploymentDate,
    ];

    public InternshipDiaryAssessmentFieldsResolver()
    {
    }

    public IEnumerable<string> CanResolveFields() => ResolvingFields;

    public Task ResolveFields(string[] fieldToResolve, InternshipDiaryTemplateResolutionContext context, CancellationToken ct)
    {
        context.SetField(InternshipDiaryTemplate.Keys.AssessmentFromEmploymentText, context.InternshipDiaryAssessment.Text);
        context.SetField(InternshipDiaryTemplate.Keys.AssessmentMarkFromEmployment, context.InternshipDiaryAssessment.Mark.ToString());
        context.SetField(InternshipDiaryTemplate.Keys.AssessmentFromEmploymentDate, InternshipDiaryTemplate.Formatting.Date(context.InternshipDiaryAssessment.Date));
        return Task.CompletedTask;
    }
}