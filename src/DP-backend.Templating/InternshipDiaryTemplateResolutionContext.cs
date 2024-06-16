using DP_backend.Domain.Employment;
using DP_backend.Domain.Templating;

namespace DP_backend.Templating;

public record InternshipDiaryTask(DateTime BeginDate, DateTime EndDate, string Name, float TimeSpentHours);

public record InternshipDiaryAssessment(int Mark, string Text, DateTime Date);

public class InternshipDiaryTemplateResolutionContext : TemplateResolutionContext
{
    public required InternshipDiaryRequest InternshipDiaryRequest { get; init; }
    public required IEnumerable<InternshipDiaryTask> InternshipDiaryTasks { get; init; }
    public required InternshipDiaryAssessment InternshipDiaryAssessment { get; init; }
}