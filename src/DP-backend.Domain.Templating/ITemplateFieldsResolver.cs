namespace DP_backend.Domain.Templating;

public interface ITemplateFieldsResolver<in TTemplateResolutionContext> where TTemplateResolutionContext : TemplateResolutionContext
{
    IEnumerable<string> CanResolveFields();
    Task ResolveFields(string[] fieldToResolve, TTemplateResolutionContext context, CancellationToken ct);
}