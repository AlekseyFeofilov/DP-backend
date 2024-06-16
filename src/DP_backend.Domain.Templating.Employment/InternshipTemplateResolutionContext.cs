using DP_backend.Domain.Employment;

namespace DP_backend.Domain.Templating.Employment;

public class InternshipTemplateResolutionContext : TemplateResolutionContext
{
    public required InternshipDiaryRequest InternshipDiaryRequest { get; init; }
}