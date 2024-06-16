namespace DP_backend.Domain.Templating;

public abstract class TemplateResolutionContext
{
    private TemplateContext? _attachedTemplateContext;

    public DocumentTemplate Template { get; internal set; }

    internal void AttachTemplateContext(TemplateContext templateContext) => _attachedTemplateContext = templateContext;

    public void SetField(string key, TemplateContext.Entry entry)
    {
        if (_attachedTemplateContext == null)
            throw new InvalidOperationException("TemplateContext hasn't been attached yet ");
        _attachedTemplateContext.Add(key, entry);
    }
}