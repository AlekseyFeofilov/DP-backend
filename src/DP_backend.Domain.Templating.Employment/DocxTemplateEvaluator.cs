using System.Xml.Linq;
using TemplateEngine.Docx;

namespace DP_backend.Domain.Templating.Employment;

public class DocxTemplateEvaluator
{
    public void Evaluate(Stream templateFileStream, TemplateContext templateContext)
    {
        var templateProcessor = new TemplateProcessor(templateFileStream);
        var content = new Content();

        foreach (var (key, value) in templateContext)
        {
            switch (value.Type)
            {
                case TemplateContext.EntryType.KeyValueCollection:
                    // todo
                    break;
                case TemplateContext.EntryType.Collection:
                    if (value.Collection?.All(x => x.Type == TemplateContext.EntryType.KeyValueCollection) == false)
                    {
                        throw new ArgumentException(null, nameof(templateContext));
                    }

                    content.Tables.Add(new TableContent(
                        name: key,
                        rows: value.Collection!.Select(
                            x => new TableRowContent(
                                x.KeyValueCollection!
                                    .Select(pair => new FieldContent(pair.Key, pair.Value.Value!))
                                    .ToList())
                        )
                    ));
                    break;
                case TemplateContext.EntryType.Value:
                    content.Fields.Add(new FieldContent(key, value.Value!));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        templateProcessor.FillContent(content);
        templateProcessor.SaveChanges(); // I suppose it will write to same stream ; todo : test
    }
}