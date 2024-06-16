namespace DP_backend.Domain.Templating;

public class TemplateContextBuilder<TTemplateResolutionContext> where TTemplateResolutionContext : TemplateResolutionContext
{
    private readonly TTemplateResolutionContext _templateResolutionContext;
    private readonly List<ITemplateFieldsResolver<TTemplateResolutionContext>> _fieldsResolvers = new();


    public TemplateContextBuilder(TTemplateResolutionContext templateResolutionContext)
    {
        _templateResolutionContext = templateResolutionContext;
    }

    public TemplateContextBuilder<TTemplateResolutionContext> AddFieldsResolver(ITemplateFieldsResolver<TTemplateResolutionContext> resolver)
    {
        _fieldsResolvers.Add(resolver);

        if (!ValidateFieldsResolvers(out var conflictingFields))
        {
            throw new InvalidOperationException($"Fields resolver {resolver} has conflicting fields [{string.Join(", ", conflictingFields)}] with already registered resolvers");
        }

        return this;
    }

    private bool ValidateFieldsResolvers(out IEnumerable<string> conflictingFields)
    {
        var fieldIds = _fieldsResolvers.SelectMany(x => x.CanResolveFields())
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(x => x.Key)
            .ToList();

        conflictingFields = fieldIds;
        return fieldIds.Count == 0;
    }

    public async Task<TemplateContext> BuildForTemplate(DocumentTemplate template, CancellationToken ct)
    {
        var templateContext = template.BaseTemplateContext?.Count > 0
            ? new TemplateContext(template.BaseTemplateContext)
            : new TemplateContext();

        _templateResolutionContext.AttachTemplateContext(templateContext);
        _templateResolutionContext.Template = template;

        var remainingFields = template.FieldIds.Except(templateContext.Keys).ToArray();

        while (remainingFields.Length > 0)
        {
            var resolvedFields = await IterateResolve(remainingFields, ct);
            if (resolvedFields.Length == 0)
            {
                throw new InvalidOperationException($"Couldn't resolve fields [{string.Join(", ", remainingFields)}]");
            }

            remainingFields = remainingFields.Except(resolvedFields).ToArray();
        }

        return templateContext;
    }

    /// <returns>resolved fields</returns>
    private async Task<string[]> IterateResolve(string[] remainingFields, CancellationToken ct)
    {
        foreach (var templateFieldsResolver in _fieldsResolvers)
        {
            var canResolveFields = templateFieldsResolver.CanResolveFields();
            var fieldsToResolve = canResolveFields.Intersect(remainingFields).ToArray();

            if (fieldsToResolve.Length == 0) continue;

            await templateFieldsResolver.ResolveFields(fieldsToResolve, _templateResolutionContext, ct);
            return fieldsToResolve;
        }

        return Array.Empty<string>();
    }
}