using System.Diagnostics.CodeAnalysis;

namespace DP_backend.Domain.Templating;

public class TemplateContext : Dictionary<string, TemplateContext.Entry>
{
    public enum EntryType
    {
        KeyValueCollection,
        Collection,
        Value
    }

    public readonly record struct Entry(
        EntryType Type,
        IReadOnlyDictionary<string, Entry>? KeyValueCollection = null,
        IReadOnlyCollection<Entry>? Collection = null,
        string? Value = null)
    {
        public static implicit operator Entry(string value) => new Entry(EntryType.Value, Value: value);
    }

    // todo create traversal func

    public TemplateContext()
    {
    }

    public TemplateContext(IEnumerable<KeyValuePair<string, Entry>> keyValuePairs) : base(keyValuePairs)
    {
    }
}